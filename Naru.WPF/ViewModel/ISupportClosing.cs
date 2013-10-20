using System;
using System.Reactive;

namespace Naru.WPF.ViewModel
{
    public interface ISupportClosing
    {
        bool CanClose();

        void Close();

        IObservable<Unit> Closed { get; }
    }
}