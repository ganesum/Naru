using Common.Logging;

using Naru.WPF.Dialog;
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

        protected WizardStepViewModel(ILog log, IDispatcherSchedulerProvider scheduler, IStandardDialog standardDialog) 
            : base(log, scheduler, standardDialog)
        {
        }

        protected abstract void LoadFromContext(TContext context);
    }
}