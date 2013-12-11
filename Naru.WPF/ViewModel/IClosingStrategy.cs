using System;
using System.Reactive;
using System.Windows.Input;

namespace Naru.WPF.ViewModel
{
    public interface IClosingStrategy
    {
        bool CanClose();

        void Close();

        Func<bool> CanCloseSetup { set; }

        IObservable<Unit> Closing { get; }

        IObservable<Unit> Closed { get; }

        ICommand CloseCommand { get; }
    }
}