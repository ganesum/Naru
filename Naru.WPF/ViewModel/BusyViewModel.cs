using System.Threading.Tasks;

using Common.Logging;

using Naru.TPL;
using Naru.WPF.Scheduler;

namespace Naru.WPF.ViewModel
{
    public class BusyViewModel : ViewModel, ISupportBusy
    {
        private readonly ISchedulerProvider _scheduler;

        #region IsActive

        private bool _isActive;

        public bool IsActive
        {
            get { return _isActive; }
            private set
            {
                if (value.Equals(_isActive)) return;
                _isActive = value;
                RaisePropertyChanged(() => IsActive);
            }
        }

        #endregion

        #region Message

        private string _message;

        public string Message
        {
            get { return _message; }
            private set
            {
                if (value == _message) return;
                _message = value;
                RaisePropertyChanged(() => Message);
            }
        }

        #endregion

        public BusyViewModel(ILog log, ISchedulerProvider scheduler) 
            : base(log)
        {
            _scheduler = scheduler;
        }

        public void Active(string message)
        {
            IsActive = true;
            Message = message;
        }

        public void InActive()
        {
            IsActive = false;
            Message = string.Empty;
        }

        public Task<Unit> ActiveAsync(string message)
        {
            return Task.Factory.StartNew(() => Active(message), _scheduler.TPL.Dispatcher)
                .Select(() => Unit.Default);
        }

        public Task<Unit> InActiveAsync()
        {
            return Task.Factory.StartNew(() => InActive(), _scheduler.TPL.Dispatcher)
                .Select(() => Unit.Default);
        }
    }
}