using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;

using Naru.Core;
using Naru.WPF.ViewModel;

namespace Naru.WPF.Command
{
    public class ObservableCommand : ObservableCommand<Unit>
    {
        public ObservableCommand()
            : base()
        {
        }

        public ObservableCommand(IObservable<bool> canExecute)
            : base(canExecute)
        {
        }
    }

    public class ObservableCommand<T> : ViewModel.ViewModel, ICommand
    {
        private bool _canExecute;
        private readonly Subject<Unit> _executed = new Subject<Unit>();

        #region Parameter

        private readonly ObservableProperty<T> _parameter = new ObservableProperty<T>();

        public T Parameter
        {
            get { return _parameter.Value; }
            set { this.RaiseAndSetIfChanged(_parameter, value); }
        }

        public IObservable<T> ParameterChanged
        {
            get
            {
                return _parameter.ValueChanged.AsObservable();
            }
        }

        #endregion

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
}