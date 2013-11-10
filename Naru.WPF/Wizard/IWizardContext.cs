using System;
using System.Reactive;

namespace Naru.WPF.Wizard
{
    public interface IWizardContext
    {
        IObservable<Unit> CanBeFinishedChanged { get; }
    }
}