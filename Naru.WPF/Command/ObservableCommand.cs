using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;

using Naru.Core;

namespace Naru.WPF.Command
{
    public class ObservableCommand : ICommand
    {
        private bool _canExecute;
        private readonly Subject<Unit> _executed = new Subject<Unit>();

        public IObservable<Unit> Executed
        {
            get { return _executed; }
        }

        public ObservableCommand()
        {
            _canExecute = true;
        }

        public ObservableCommand(IObservable<bool> canExecute)
        {
            canExecute
                .DistinctUntilChanged()
                .Subscribe(x =>
                {
                    _canExecute = x;

                    CanExecuteChanged.SafeInvoke(this);
                });

            _canExecute = false;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public void Execute(object parameter)
        {
            _executed.OnNext(Unit.Default);
        }

        public event EventHandler CanExecuteChanged;
    }

    public class ObservableCommand<T> : ICommand
    {
        private bool _canExecute;
        private readonly Subject<T> _executed = new Subject<T>();

        public IObservable<T> Executed
        {
            get { return _executed; }
        }

        public ObservableCommand()
        {
            _canExecute = true;
        }

        public ObservableCommand(IObservable<bool> canExecute)
        {
            canExecute
                .DistinctUntilChanged()
                .Subscribe(x =>
                           {
                               _canExecute = x;

                               CanExecuteChanged.SafeInvoke(this);
                           });

            _canExecute = false;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public void Execute(object parameter)
        {
            _executed.OnNext((T)parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}