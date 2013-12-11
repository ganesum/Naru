using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Naru.RX;
using Naru.TPL;
using Naru.WPF.Scheduler;

namespace Naru.WPF.ViewModel
{
    public class BusyViewModel : ViewModel, IBusyViewModel
    {
        private readonly ISchedulerProvider _scheduler;
        private readonly BusyLatch _busyLatch = new BusyLatch();

        #region IsActive

        private readonly ObservableProperty<bool> _isActive = new ObservableProperty<bool>();

        public bool IsActive
        {
            get { return _isActive.Value; }
            private set { _isActive.RaiseAndSetIfChanged(value); }
        }

        public IObservable<bool> IsActiveChanged
        {
            get { return _isActive.ValueChanged.DistinctUntilChanged(); }
        }

        #endregion

        #region Message

        private readonly ObservableProperty<string> _message = new ObservableProperty<string>();

        public string Message
        {
            get { return _message.Value; }
            private set { _message.RaiseAndSetIfChanged(value); }
        }

        #endregion

        public BusyLatch BusyLatch
        {
            get { return _busyLatch; }
        }

        public BusyViewModel(ISchedulerProvider scheduler)
        {
            _scheduler = scheduler;

            _isActive.ConnectINPCProperty(this, () => IsActive, scheduler).AddDisposable(Disposables);
            _message.ConnectINPCProperty(this, () => Message, scheduler).AddDisposable(Disposables);

            _busyLatch.AddDisposable(Disposables);
            _busyLatch.IsActive.Subscribe(x => IsActive = x);
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

        public Task ActiveAsync(string message)
        {
            return Task.Factory.StartNew(() => Active(message), _scheduler.Dispatcher.TPL);
        }

        public Task InActiveAsync()
        {
            return Task.Factory.StartNew(() => InActive(), _scheduler.Dispatcher.TPL);
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}