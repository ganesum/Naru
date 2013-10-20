using System;

namespace Naru.WPF.ViewModel
{
    public interface ISupportBusy
    {
        IObservable<bool> IsActiveChanged { get; }

        string Message { get; }

        void Active(string message);

        void InActive();
    }
}