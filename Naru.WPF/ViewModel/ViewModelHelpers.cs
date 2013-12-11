using System;
using System.Linq.Expressions;
using System.Reactive.Linq;

using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.ToolBar;

namespace Naru.WPF.ViewModel
{
    public static class ViewModelHelpers
    {
        public static void SetupHeader(this ISupportHeader viewModel, ISchedulerProvider scheduler, string displayName = null, string uri = null)
        {
            var headerViewModel = new HeaderViewModel(scheduler)
            {
                DisplayName = displayName,
                ImageName = uri
            };

            viewModel.SetupHeader(headerViewModel);
        }

        public static IDisposable SyncViewModelActivationStates(this ISupportActivationState source, ISupportActivationState viewModel)
        {
            return source.ActivationStateViewModel.ActivationStateChanged
                         .Subscribe(x =>
                         {
                             if (x)
                                 viewModel.ActivationStateViewModel.Activate();
                             else
                                 viewModel.ActivationStateViewModel.DeActivate();
                         });
        }

        public static IDisposable SyncToolBarItemWithViewModelActivationState(this ISupportActivationState source, params IToolBarItem[] toolBarItems)
        {
            foreach (var toolBarItem in toolBarItems)
            {
                toolBarItem.IsVisible = source.ActivationStateViewModel.IsActive;
            }

            return source.ActivationStateViewModel.ActivationStateChanged
                         .Subscribe(x =>
                         {
                             foreach (var toolBarItem in toolBarItems)
                             {
                                 toolBarItem.IsVisible = x;
                             }
                         });
        }

        public static IDisposable SyncViewModelBusy(this ISupportBusy destination, ISupportBusy source)
        {
            return source.BusyViewModel.IsActiveChanged
                         .Subscribe(x =>
                         {
                             if (x)
                             {
                                 destination.BusyViewModel.Active(source.BusyViewModel.Message);
                             }
                             else
                             {
                                 destination.BusyViewModel.InActive();
                             }
                         });
        }

        public static IDisposable SyncViewModelClose(this ISupportClosing destination, ISupportClosing source)
        {
            return source.ClosingStrategy.Closed
                         .Subscribe(x => destination.ClosingStrategy.Close());
        }

        public static void ExecuteOnClosed(this ISupportClosing source, Action action)
        {
            IDisposable supportClosingClosed = null;
            supportClosingClosed = source.ClosingStrategy.Closed
                                         .Subscribe(x =>
                                         {
                                             action();

                                             if (supportClosingClosed != null)
                                             {
                                                 supportClosingClosed.Dispose();
                                             }
                                         });
        }

        public static object GetViewAndBind<TViewModel>(this TViewModel viewModel)
            where TViewModel : IViewModel
        {
            var view = ViewServiceHelper.CreateView(viewModel.GetType());

            ViewServiceHelper.BindViewModel(view, viewModel);

            return view;
        }
    }
}