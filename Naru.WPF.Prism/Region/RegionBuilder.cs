using System;
using System.Threading.Tasks;
using System.Windows;

using Common.Logging;

using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

using Naru.TPL;
using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;

namespace Naru.WPF.Prism.Region
{
    public class RegionBuilder : IRegionBuilder
    {
        private readonly ILog _log;
        private readonly Func<IRegionManager> _regionManagerFactory;
        private readonly IScheduler _scheduler;

        public RegionBuilder(ILog log, Func<IRegionManager> regionManagerFactory, IScheduler scheduler)
        {
            _log = log;
            _regionManagerFactory = regionManagerFactory;
            _scheduler = scheduler;
        }

        public void Clear(string regionName)
        {
            _log.Debug(string.Format("Clearing region {0}", regionName));

            _scheduler.Dispatcher.ExecuteSync(() => ClearInternal(regionName));
        }

        public Task ClearAsync(string regionName)
        {
            _log.Debug(string.Format("Clearing region {0}", regionName));

            return Task.Factory.StartNew(() => ClearInternal(regionName), _scheduler.Dispatcher);
        }

        private void ClearInternal(string regionName)
        {
            var regionManager = _regionManagerFactory();
            var region = regionManager.Regions[regionName];

            foreach (var obj in region.Views)
            {
                var view = obj as FrameworkElement;
                if (view == null) continue;

                var viewModel = view.DataContext as IViewModel;
                if (viewModel == null) continue;

                view.DataContext = null;

                var closableViewModel = viewModel as ISupportClosing;
                if (closableViewModel != null)
                    closableViewModel.Close();

                region.Remove(obj);
            }
        }
    }

    public class RegionBuilder<TViewModel> : IRegionBuilder<TViewModel>
            where TViewModel : IViewModel
    {
        private readonly ILog _log;
        private readonly Func<IRegionManager> _regionManagerFactory;
        private readonly IUnityContainer _container;
        private readonly IScheduler _scheduler;

        private bool _scope;
        private Action<TViewModel> _initialiseViewModel;

        public RegionBuilder(ILog log, Func<IRegionManager> regionManagerFactory, IUnityContainer container, IScheduler scheduler)
        {
            _log = log;
            _regionManagerFactory = regionManagerFactory;
            _container = container;
            _scheduler = scheduler;
        }

        public IRegionBuilder<TViewModel> WithScope()
        {
            _scope = true;

            return this;
        }

        public IRegionBuilder<TViewModel> WithInitialisation(Action<TViewModel> initialiseViewModel)
        {
            _initialiseViewModel = initialiseViewModel;

            return this;
        }

        public void Show(string regionName, TViewModel viewModel)
        {
            _log.Debug(string.Format("Scope = {0}", _scope));
            var container = ViewService.GetContainer(_container, _scope);

            _scheduler.Dispatcher.ExecuteSync(() => ShowInternal(regionName, viewModel, container));
        }

        public Task ShowAsync(string regionName, TViewModel viewModel)
        {
            _log.Debug(string.Format("Scope = {0}", _scope));
            var container = ViewService.GetContainer(_container, _scope);

            return Task.Factory.StartNew(() => ShowInternal(regionName, viewModel, container), _scheduler.Dispatcher);
        }

        public TViewModel Show(string regionName)
        {
            _log.Debug(string.Format("Scope = {0}", _scope));
            var container = ViewService.GetContainer(_container, _scope);

            _log.Debug(string.Format("Creating ViewModel - {0}", typeof(TViewModel).FullName));
            var viewModel = ViewService.CreateViewModel<TViewModel>(container);

            _scheduler.Dispatcher.ExecuteSync(() => ShowInternal(regionName, viewModel, container));

            return viewModel;
        }

        public Task<TViewModel> ShowAsync(string regionName)
        {
            _log.Debug(string.Format("Scope = {0}", _scope));
            var container = ViewService.GetContainer(_container, _scope);

            _log.Debug(string.Format("Creating ViewModel - {0}", typeof(TViewModel).FullName));
            var viewModel = ViewService.CreateViewModel<TViewModel>(container);

            return Task.Factory
                .StartNew(() => ShowInternal(regionName, viewModel, container), _scheduler.Dispatcher)
                .Select(() => viewModel);
        }

        private void ShowInternal(string regionName, TViewModel viewModel, IUnityContainer container)
        {
            _log.Debug(string.Format("Creating View for ViewModel - {0}", viewModel.GetType().FullName));
            var view = ViewService.CreateView(viewModel.GetType());

            _log.Debug(string.Format("Binding View and ViewModel - {0}", viewModel.GetType().FullName));
            ViewService.BindViewModel(view, viewModel);

            if (_initialiseViewModel != null)
                _initialiseViewModel(viewModel);

            _log.Debug(string.Format("Adding View and ViewModel - {0} - to Region - {1}", viewModel.GetType().FullName, regionName));
            var regionManager = AddToRegion(_regionManagerFactory, regionName, view, _scope);

            if (_scope)
            {
                container.RegisterInstance(regionManager);
            }
        }

        private static IRegionManager AddToRegion(Func<IRegionManager> regionManagerFactory, string regionName, FrameworkElement view, bool scoped = false)
        {
            var regionManager = regionManagerFactory();
            var scopedRegionManager = regionManager.Regions[regionName].Add(view, null, scoped);
            regionManager.Regions[regionName].Activate(view);

            return scopedRegionManager;
        }
    }
}