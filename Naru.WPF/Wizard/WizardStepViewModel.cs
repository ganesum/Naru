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

        #region Context

        private TContext _context;

        public TContext Context
        {
            protected get { return _context; }
            set
            {
                _context = value;
                LoadFromContext(Context);
            }
        }

        #endregion

        protected WizardStepViewModel(ILog log, ISchedulerProvider scheduler, IViewService viewService) 
            : base(log, scheduler, viewService)
        {
        }

        protected abstract void LoadFromContext(TContext context);
    }
}