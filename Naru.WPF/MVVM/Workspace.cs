using System;
using System.Threading.Tasks;

using Common.Logging;

using Naru.Core;
using Naru.TPL;
using Naru.WPF.Scheduler;

namespace Naru.WPF.MVVM
{
    public abstract class Workspace : ViewModel, ISupportClosing, ISupportActivationState, ISupportVisibility, ISupportHeader, ISupportInitialisation
    {
        protected readonly IScheduler Scheduler;
        protected readonly IViewService ViewService;
        protected readonly CompositeDisposable Disposables;

        public BusyViewModel BusyViewModel { get; private set; }

        protected Workspace(ILog log, IScheduler scheduler, IViewService viewService) 
            : base(log)
        {
            Scheduler = scheduler;
            ViewService = viewService;

            BusyViewModel = new BusyViewModel(log, scheduler);

            Disposables = new CompositeDisposable();

            CloseCommand = new DelegateCommand(Close);

            Show();
        }

        #region SupportClosing

        public DelegateCommand CloseCommand { get; private set; }

        public virtual bool CanClose()
        {
            return false;
        }

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

            BusyViewModel.ActiveAsync("... Initialising ...")
                .SelectMany(() =>
                {
                    OnInitialise();

                    Initialised.SafeInvoke(this);
                    _onInitialiseHasBeenCalled = true;
                }, Scheduler.Task)
                .LogException(Log)
                .CatchAndHandle(ex => ViewService
                    .StandardDialogBuilder()
                    .Error("Error", string.Format("Exception in OnInitialise() call. {0}", ex.Message)), Scheduler.Task)
                .Finally(BusyViewModel.InActive, Scheduler.Task);
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

        protected virtual Task OnInitialise()
        {
            return CompletedTask.Default;
        }

        protected virtual void OnActivate()
        { }

        protected virtual void OnDeActivate()
        { }

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

        #region SupportInitialisation

        Task ISupportInitialisation.OnInitialise()
        {
            return OnInitialise();
        }

        #endregion
    }
}