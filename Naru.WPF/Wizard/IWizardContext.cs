using System;

using Naru.TPL;

namespace Naru.WPF.Wizard
{
    public interface IWizardContext
    {
        IObservable<Unit> CanBeFinishedChanged { get; }
    }
}