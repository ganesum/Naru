using System;
using System.Reactive;
using System.Reactive.Subjects;
using System.Windows.Input;

using Naru.Core;

namespace Naru.RX
{
    public class ObservableCommand : ICommand
    {
        private readonly Func<bool> _canExecute;
        private readonly Subject<Unit> _executed = new Subject<Unit>();

        public IObservable<Unit> Executed
        {
            get { return _executed; }
        }

        public ObservableCommand()
            : this(() => true)
        {
        }

        public ObservableCommand(Func<bool> canExecute)
        {
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute();
        }

        public void Execute(object parameter)
        {
            _executed.OnNext(Unit.Default);
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged.SafeInvoke(this);
        }
    }

    public class ObservableCommand<T> : ICommand
    {
        private readonly Func<T, bool> _canExecute;
        private readonly Subject<T> _executed = new Subject<T>();

        public IObservable<T> Executed
        {
            get { return _executed; }
        }

        public ObservableCommand()
            : this(_ => true)
        {
        }

        public ObservableCommand(Func<T, bool> canExecute)
        {
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _executed.OnNext((T)parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged.SafeInvoke(this);
        }
    }
}