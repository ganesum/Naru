using System;

namespace Naru.WPF.ViewModel
{
    public interface IBusyViewModel
    {
        IObservable<bool> IsActiveChanged { get; }

        string Message { get; }

        void Active(string message);

        void InActive();
    }
}