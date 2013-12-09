using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

using Common.Logging;

using Naru.TPL;
using Naru.WPF.Command;
using Naru.WPF.Dialog;
using Naru.WPF.Scheduler;

namespace Naru.WPF.ViewModel
{
    public abstract class Workspace : ViewModel, ISupportClosing, ISupportVisibility, ISupportHeader, ISupportActivationState, ISupportBusy
    {
        protected readonly ILog Log;
        protected readonly ISchedulerProvider Scheduler;
        protected readonly IStandardDialog StandardDialog;

        private readonly Subject<Unit> _closed = new Subject<Unit>();
        private readonly Subject<bool> _isVisibleChanged = new Subject<bool>();

        public BusyViewModel BusyViewModel { get; private set; }

        public IActivationStateViewModel ActivationStateViewModel { get; private set; }

        protected Workspace(ILog log, ISchedulerProvider scheduler, IStandardDialog standardDialog)
        {
            Log = log;
            Scheduler = scheduler;
            StandardDialog = standardDialog;

            BusyViewModel = new BusyViewModel(scheduler);

            ActivationStateViewModel = new ActivationStateViewModel(log, scheduler);
            ActivationStateViewModel.OnInitialise
                                    .SelectMany(_ => OnInitialise().ToObservable()
                                                                   .TakeUntil(BusyViewModel.BusyLatch))
                                    .TakeUntil(Closed)
                                    .Subscribe(_ => { });

            ActivationStateViewModel.ActivationStateChanged
                                    .ObserveOn(scheduler.Dispatcher.RX)
                                    .Where(isActive => isActive)
                                    .TakeUntil(Closed)
                                    .Subscribe(_ => OnActivate());

            ActivationStateViewModel.ActivationStateChanged
                                    .ObserveOn(scheduler.Dispatcher.RX)
                                    .Where(isActive => !isActive)
                                    .TakeUntil(Closed)
                                    .Subscribe(_ => OnDeActivate());

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
    }
}