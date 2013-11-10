using Naru.WPF.ViewModel;

namespace Naru.WPF.Wizard
{
    public interface IWizardStepViewModel<TContext> : IViewModel, ISupportActivationState
        where TContext : IWizardContext
    {
        int Ordinal { get; set; }

        TContext Context { set; }

        BusyViewModel BusyViewModel { get; }
    }
}