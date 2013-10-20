using System;

namespace Naru.WPF.ViewModel
{
    public interface ISupportActivationState
    {
        bool IsActive { get; }

        void Activate();

        void DeActivate();

        IObservable<bool> ActivationStateChanged { get; }

        event EventHandler Initialised;
    }
}