using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;

using Common.Logging;

using Naru.RX;
using Naru.WPF.Command;

namespace Naru.WPF.ViewModel
{
    public class ClosingStrategy : IClosingStrategy
    {
        private readonly ILog _log;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private readonly Subject<Unit> _closing = new Subject<Unit>();
        private readonly Subject<Unit> _closed = new Subject<Unit>();

        private Func<bool> _canClose;

        public ICommand CloseCommand { get; private set; }

        public Func<bool> CanCloseSetup
        {
            set { _canClose = value; }
        }

        public ClosingStrategy(ILog log)
        {
            _log = log;

            _closing.AddDisposable(_disposable);
            _closed.AddDisposable(_disposable);

            CanCloseSetup = () => true;

            CloseCommand = new DelegateCommand(() => Close());
        }

        public bool CanClose()
        {
            return _canClose();
        }

        public void Close()
        {
            _log.Debug(string.Format("Closing ViewModel {0}", GetType().FullName));

            _closing.OnNext(Unit.Default);

            _closed.OnNext(Unit.Default);
        }

        public IObservable<Unit> Closing { get { return _closing.AsObservable(); } }

        public IObservable<Unit> Closed { get { return _closed.AsObservable(); } }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}