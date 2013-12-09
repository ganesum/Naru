using System;
using System.Reactive;

namespace Naru.WPF.ViewModel
{
    public interface IActivationStateViewModel
    {
        bool IsActive { get; }

        void Activate();

        void DeActivate();

        IObservable<bool> ActivationStateChanged { get; }

        IObservable<Unit> OnInitialise { get; }
    }
}