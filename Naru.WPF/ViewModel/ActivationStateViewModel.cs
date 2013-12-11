using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using Common.Logging;

using Naru.RX;
using Naru.WPF.Scheduler;

namespace Naru.WPF.ViewModel
{
    public class ActivationStateViewModel : ViewModel, IActivationStateViewModel
    {
        private readonly ILog _log;
        private readonly Subject<Unit> _onInitialise = new Subject<Unit>();

        private bool _onInitialiseHasBeenCalled;

        #region IsActive

        private readonly ObservableProperty<bool> _isActive = new ObservableProperty<bool>();

        public bool IsActive
        {
            get { return _isActive.Value; }
            set { _isActive.RaiseAndSetIfChanged(value); }
        }

        #endregion

        public IObservable<bool> ActivationStateChanged { get { return _isActive.ValueChanged.AsObservable().DistinctUntilChanged(); } }

        public IObservable<Unit> OnInitialise { get { return _onInitialise.AsObservable(); } }

        public ActivationStateViewModel(ILog log, ISchedulerProvider scheduler)
        {
            _log = log;
            _isActive.ConnectINPCProperty(this, () => IsActive, scheduler).AddDisposable(Disposables);
        }

        public void Activate()
        {
            _log.Debug(string.Format("Activate called on {0}", GetType().FullName));
            _log.Debug(string.Format("Active value - {0}", IsActive));

            if (IsActive) return;

            IsActive = true;
            _log.Debug(string.Format("Active value - {0}", IsActive));

            if (_onInitialiseHasBeenCalled) return;

            _log.Debug(string.Format("Calling OnInitialise on {0}", GetType().FullName));

            _onInitialise.OnNext(Unit.Default);
            _onInitialise.OnCompleted();

            _onInitialiseHasBeenCalled = true;
        }

        public void DeActivate()
        {
            _log.Debug(string.Format("DeActivate called on {0}", GetType().FullName));
            _log.Debug(string.Format("DeActivate value - {0}", IsActive));

            if (!IsActive) return;

            IsActive = false;
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}