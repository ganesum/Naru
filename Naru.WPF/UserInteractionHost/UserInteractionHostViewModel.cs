using System;

using Common.Logging;

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

        private IViewModel _viewModel;

        public IViewModel ViewModel
        {
            get { return _viewModel; }
            private set
            {
                _viewModel = value;
                RaisePropertyChanged(() => ViewModel);
            }
        }

        #endregion

        #region ShowClose

        private bool _showClose;

        public bool ShowClose
        {
            get { return _showClose; }
            private set { _showClose = value; }
        }

        #endregion

        public UserInteractionHostViewModel(ILog log, ISchedulerProvider scheduler, IStandardDialog standardDialog)
            : base(log, scheduler, standardDialog)
        {
        }

        public void Initialise(IViewModel viewModel)
        {
            ViewModel = viewModel;
            
            var supportClosing = ViewModel as ISupportClosing;
            if (supportClosing != null)
            {
                ShowClose = true;

                IDisposable closing = null;
                closing = supportClosing.Closed
                                        .Subscribe(x =>
                                                   {
                                                       if (!_viewModelIsClosed)
                                                       {
                                                           _viewModelIsClosed = true;
                                                           Close();
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
                supportClosing.Close();
            }
        }
    }
}