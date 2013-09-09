using System;
using System.Threading.Tasks;

using Common.Logging;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;

using Naru.WPF.TPL;

namespace Naru.WPF.MVVM
{
    public abstract class Workspace : ViewModel, ISupportClosing, ISupportActivationState
    {
        protected readonly IScheduler Scheduler;
        protected readonly CompositeDisposable Disposables;

        #region IsBusy

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            private set
            {
                if (value.Equals(_isBusy)) return;
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        #endregion

        #region BusyMessage

        private string _busyMessage;

        public string BusyMessage
        {
            get { return _busyMessage; }
            private set
            {
                if (value == _busyMessage) return;
                _busyMessage = value;
                RaisePropertyChanged(() => BusyMessage);
            }
        }

        #endregion

        public DelegateCommand ClosingCommand { get; private set; }

        protected Workspace(ILog log, IScheduler scheduler) 
            : base(log)
        {
            Scheduler = scheduler;
            Disposables = new CompositeDisposable();

            ClosingCommand = new DelegateCommand(Close);
        }

        #region SupportClosing

        public void Close()
        {
            Log.Debug(string.Format("Closing ViewModel {0} - {1}", GetType().FullName, DisplayName));

            Closing();

            Disposables.Dispose();

            CleanUp();

            OnClosed.SafeInvoke(this);
        }

        public event EventHandler OnClosed;

        protected virtual void Closing()
        { }

        protected virtual void CleanUp()
        { }

        #endregion

        protected void Busy(string message)
        {
            IsBusy = true;
            BusyMessage = message;
        }

        protected void Idle()
        {
            IsBusy = false;
            BusyMessage = string.Empty;
        }

        protected Task<Unit> BusyAsync(string message)
        {
            return Task.Factory
                .StartNew(() => Busy(message), Scheduler.Dispatcher)
                .Select(() => Unit.Default);
        }

        protected Task<Unit> IdleAsync()
        {
            return Task.Factory
                .StartNew(Idle, Scheduler.Dispatcher)
                .Select(() => Unit.Default);
        }

        #region SupportActivationState

        private bool _onInitialiseHasBeenCalled;

        public bool IsActive { get; private set; }

        void ISupportActivationState.Activate()
        {
            Log.Debug(string.Format("Activate called on {0} - {1}", GetType().FullName, DisplayName));
            Log.Debug(string.Format("Active value - {0}", IsActive));
            if (IsActive) return;

            IsActive = true;
            Log.Debug(string.Format("Active value - {0}", IsActive));

            OnActivationStateChanged.SafeInvoke(this, new DataEventArgs<bool>(IsActive));

            OnActivate();

            if (_onInitialiseHasBeenCalled) return;

            Log.Debug(string.Format("Calling OnInitialise on {0} - {1}", GetType().FullName, DisplayName));
            OnInitialise();
            OnInitialised.SafeInvoke(this);
            _onInitialiseHasBeenCalled = true;
        }

        void ISupportActivationState.DeActivate()
        {
            IsActive = false;

            Log.Debug(string.Format("DeActivate called on {0} - {1}", GetType().FullName, DisplayName));
            Log.Debug(string.Format("DeActivate value - {0}", IsActive));

            OnActivationStateChanged.SafeInvoke(this, new DataEventArgs<bool>(IsActive));

            OnDeActivate();
        }

        public event EventHandler<DataEventArgs<bool>>  OnActivationStateChanged;

        public event EventHandler OnInitialised;

        protected virtual void OnInitialise()
        { }

        protected virtual void OnActivate()
        { }

        protected virtual void OnDeActivate()
        { }

        #endregion
    }
}