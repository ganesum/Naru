using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

using Common.Logging;

using Naru.Core;
using Naru.TPL;
using Naru.WPF.Command;
using Naru.WPF.Core;
using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;

using Unit = System.Reactive.Unit;

namespace Naru.WPF.ViewModel
{
    public abstract class Workspace : ViewModel, ISupportClosing, ISupportActivationState, ISupportVisibility, ISupportHeader, ISupportInitialisation
    {
        protected readonly ISchedulerProvider Scheduler;
        protected readonly IViewService ViewService;
        protected readonly CompositeDisposable Disposables;

        private readonly Subject<bool> _activationStateChanged = new Subject<bool>();
        private readonly Subject<Unit> _closed = new Subject<Unit>();
        private readonly Subject<bool> _isVisibleChanged = new Subject<bool>();

        public BusyViewModel BusyViewModel { get; private set; }

        protected Workspace(ILog log, ISchedulerProvider scheduler, IViewService viewService) 
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

            _closed.OnNext(Unit.Default);
        }

        public IObservable<Unit> Closed { get { return _closed.AsObservable(); } }

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

            _activationStateChanged.OnNext(IsActive);

            OnActivate();

            if (_onInitialiseHasBeenCalled) return;

            Log.Debug(string.Format("Calling OnInitialise on {0} - {1}", GetType().FullName, Header));

            BusyViewModel.ActiveAsync("... Initialising ...")
                .Then(() =>
                {
                    OnInitialise();

                    Initialised.SafeInvoke(this);
                    _onInitialiseHasBeenCalled = true;
                }, Scheduler.Task.TPL)
                .LogException(Log)
                .CatchAndHandle(ex => ViewService
                    .StandardDialog()
                    .Error("Error", string.Format("Exception in OnInitialise() call. {0}", ex.Message)), Scheduler.Task.TPL)
                .Finally(BusyViewModel.InActive, Scheduler.Task.TPL);
        }

        void ISupportActivationState.DeActivate()
        {
            IsActive = false;

            Log.Debug(string.Format("DeActivate called on {0} - {1}", GetType().FullName, Header));
            Log.Debug(string.Format("DeActivate value - {0}", IsActive));

            _activationStateChanged.OnNext(IsActive);

            OnDeActivate();
        }

        public IObservable<bool> ActivationStateChanged{get { return _activationStateChanged.AsObservable(); }}

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

                _isVisibleChanged.OnNext(IsVisible);
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

        public IObservable<bool> IsVisibleChanged{get { return _isVisibleChanged.AsObservable(); }}

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