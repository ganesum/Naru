using System;
using System.Threading.Tasks;

using Common.Logging;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;

using Naru.WPF.TPL;

namespace Naru.WPF.MVVM
{
    public abstract class Workspace : ViewModel, ISupportClosing, ISupportActivationState, ISupportAsync, ISupportVisibility, ISupportHeader
    {
        protected readonly IScheduler Scheduler;
        protected readonly CompositeDisposable Disposables;

        protected Workspace(ILog log, IScheduler scheduler) 
            : base(log)
        {
            Scheduler = scheduler;
            Disposables = new CompositeDisposable();

            ClosingCommand = new DelegateCommand(Close);

            Show();
        }

        #region SupportClosing

        public DelegateCommand ClosingCommand { get; private set; }

        public virtual bool CanClose()
        {
            return false;
        }

        public event EventHandler CanCloseChanged;

        public void Close()
        {
            Log.Debug(string.Format("Closing ViewModel {0} - {1}", GetType().FullName, Header));

            Closing();

            Disposables.Dispose();

            CleanUp();

            Closed.SafeInvoke(this);
        }

        public event EventHandler Closed;

        protected virtual void Closing()
        { }

        protected virtual void CleanUp()
        { }

        #endregion

        #region SupportActivationState

        private bool _onInitialiseHasBeenCalled;

        public bool IsActive { get; private set; }

        void ISupportActivationState.Activate()
        {
            Log.Debug(string.Format("Activate called on {0} - {1}", GetType().FullName, Header));
            Log.Debug(string.Format("Active value - {0}", IsActive));
            if (IsActive) return;

            IsActive = true;
            Log.Debug(string.Format("Active value - {0}", IsActive));

            ActivationStateChanged.SafeInvoke(this, new DataEventArgs<bool>(IsActive));

            OnActivate();

            if (_onInitialiseHasBeenCalled) return;

            Log.Debug(string.Format("Calling OnInitialise on {0} - {1}", GetType().FullName, Header));
            OnInitialise();
            Initialised.SafeInvoke(this);
            _onInitialiseHasBeenCalled = true;
        }

        void ISupportActivationState.DeActivate()
        {
            IsActive = false;

            Log.Debug(string.Format("DeActivate called on {0} - {1}", GetType().FullName, Header));
            Log.Debug(string.Format("DeActivate value - {0}", IsActive));

            ActivationStateChanged.SafeInvoke(this, new DataEventArgs<bool>(IsActive));

            OnDeActivate();
        }

        public event EventHandler<DataEventArgs<bool>>  ActivationStateChanged;

        public event EventHandler Initialised;

        protected virtual void OnInitialise()
        { }

        protected virtual void OnActivate()
        { }

        protected virtual void OnDeActivate()
        { }

        #endregion

        #region SupportAsync

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

        public void Busy(string message)
        {
            IsBusy = true;
            BusyMessage = message;
        }

        public void Idle()
        {
            IsBusy = false;
            BusyMessage = string.Empty;
        }

        public Task<Unit> BusyAsync(string message)
        {
            return Task.Factory.StartNew(() => Busy(message), Scheduler.Dispatcher)
                .Select(() => Unit.Default);
        }

        public Task<Unit> IdleAsync()
        {
            return Task.Factory.StartNew(() => Idle(), Scheduler.Dispatcher)
                .Select(() => Unit.Default);
        }

        #endregion

        #region SupportVisibility

        #region IsVisible

        private bool _isVisible;

        public bool IsVisible
        {
            get { return _isVisible; }
            private set
            {
                if (value.Equals(_isVisible)) return;
                _isVisible = value;
                RaisePropertyChanged(() => IsVisible);

                IsVisibleChanged.SafeInvoke(this, new DataEventArgs<bool>(IsVisible));
            }
        }

        #endregion

        public void Show()
        {
            IsVisible = true;
        }

        public void Hide()
        {
            IsVisible = false;
        }

        public event EventHandler<DataEventArgs<bool>> IsVisibleChanged;

        #endregion

        #region SupportHeader

        public IViewModel Header { get; protected set; }

        void ISupportHeader.SetupHeader(IViewModel headerViewModel)
        {
            Header = headerViewModel;
        }

        #endregion
    }
}