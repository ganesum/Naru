using Common.Logging;

using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.ViewModel;

namespace Naru.WPF.Wizard
{
    public abstract class WizardStepViewModel<TContext> : Workspace, IWizardStepViewModel<TContext> 
        where TContext : IWizardContext, new()
    {
        public int Ordinal { get; set; }

        public abstract string Name { get; }

        public TContext Context { set; get; }

        protected WizardStepViewModel(ILog log, ISchedulerProvider scheduler, IViewService viewService) 
            : base(log, scheduler, viewService)
        {
        }
    }
}