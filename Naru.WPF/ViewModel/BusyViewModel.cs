using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

using Common.Logging;

using Naru.TPL;
using Naru.WPF.Scheduler;

namespace Naru.WPF.ViewModel
{
    public class BusyViewModel : ViewModel, ISupportBusy
    {
        private readonly ISchedulerProvider _scheduler;
        private readonly Subject<bool> _isActiveChanged = new Subject<bool>(); 

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

                _isActiveChanged.OnNext(value);
            }
        }

        #endregion

        public IObservable<bool> IsActiveChanged
        {
            get { return _isActiveChanged.AsObservable(); }
        }

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
            return Task.Factory.StartNew(() => Active(message), _scheduler.Dispatcher.TPL)
                .Select(() => Unit.Default);
        }

        public Task<Unit> InActiveAsync()
        {
            return Task.Factory.StartNew(() => InActive(), _scheduler.Dispatcher.TPL)
                .Select(() => Unit.Default);
        }
    }
}