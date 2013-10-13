using System;

using Naru.Core;

namespace Naru.WPF.MVVM
{
    public interface ISupportActivationState
    {
        bool IsActive { get; }

        void Activate();

        void DeActivate();

        event EventHandler<DataEventArgs<bool>>  ActivationStateChanged;

        event EventHandler Initialised;
    }
}