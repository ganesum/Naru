using System;

using Common.Logging;

using Naru.RX;
using Naru.WPF.Dialog;
using Naru.WPF.Scheduler;
using Naru.WPF.ViewModel;

namespace Naru.WPF.UserInteractionHost
{
    public interface IUserInteractionHostViewModel : IViewModel, ISupportClosing
    {
        void Initialise(IViewModel viewModel);
    }

    public class UserInteractionHostViewModel : Workspace, IUserInteractionHostViewModel
    {
        private bool _viewModelIsClosed;

        #region ViewModel

        private readonly ObservableProperty<IViewModel> _viewModel = new ObservableProperty<IViewModel>();

        public IViewModel ViewModel
        {
            get { return _viewModel.Value; }
            private set { _viewModel.RaiseAndSetIfChanged(value); }
        }

        #endregion

        #region ShowClose

        private readonly ObservableProperty<bool> _showClose = new ObservableProperty<bool>();

        public bool ShowClose
        {
            get { return _showClose.Value; }
            private set { _showClose.RaiseAndSetIfChanged(value); }
        }

        #endregion

        public UserInteractionHostViewModel(ILog log, IDispatcherSchedulerProvider scheduler, IStandardDialog standardDialog)
            : base(log, scheduler, standardDialog)
        {
            _viewModel.ConnectINPCProperty(this, () => ViewModel, scheduler).AddDisposable(Disposables);
            _showClose.ConnectINPCProperty(this, () => ShowClose, scheduler).AddDisposable(Disposables);
        }

        public void Initialise(IViewModel viewModel)
        {
            ViewModel = viewModel;
            
            var supportClosing = ViewModel as ISupportClosing;
            if (supportClosing != null)
            {
                ShowClose = true;

                IDisposable closing = null;
                closing = supportClosing.ClosingStrategy.Closed
                                        .Subscribe(x =>
                                                   {
                                                       if (!_viewModelIsClosed)
                                                       {
                                                           _viewModelIsClosed = true;
                                                           ClosingStrategy.Close();
                                                       }

                                                       if (closing != null)
                                                       {
                                                           closing.Dispose();
                                                       }
                                                   });
            }
        }

        protected override void CleanUp()
        {
            var supportClosing = ViewModel as ISupportClosing;
            if (supportClosing == null)
            {
                return;
            }

            if (!_viewModelIsClosed)
            {
                _viewModelIsClosed = true;
                supportClosing.ClosingStrategy.Close();
            }
        }
    }
}