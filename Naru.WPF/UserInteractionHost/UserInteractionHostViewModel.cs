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

        public UserInteractionHostViewModel(ILog log, ISchedulerProvider scheduler, IStandardDialog standardDialog)
            : base(log, scheduler, standardDialog)
        {
        }

        public void Initialise(IViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        protected override void CleanUp()
        {
            var supportClosing = ViewModel as ISupportClosing;
            if (supportClosing == null)
            {
                return;
            }

            supportClosing.Close();
        }
    }
}