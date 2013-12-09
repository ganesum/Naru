using Naru.WPF.ViewModel;

namespace Naru.WPF.Wizard
{
    public interface IWizardStepViewModel<TContext> : IViewModel, ISupportActivationState, ISupportBusy
        where TContext : IWizardContext
    {
        int Ordinal { get; set; }

        TContext Context { set; }
    }
}