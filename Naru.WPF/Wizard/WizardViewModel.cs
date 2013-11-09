using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Common.Logging;

using Naru.TPL;
using Naru.WPF.Command;
using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.ViewModel;

namespace Naru.WPF.Wizard
{
    public abstract class WizardViewModel<TContext> : Workspace, INavigation
        where TContext : IWizardContext, new()
    {
        private readonly TContext _context = new TContext();
        private readonly Dictionary<WizardStepIdentifier, IWizardStepViewModel<TContext>> _steps = new Dictionary<WizardStepIdentifier, IWizardStepViewModel<TContext>>();

        private bool _canBeFinished;

        #region CurrentStep

        private IWizardStepViewModel<TContext> _currentStep;

        public IWizardStepViewModel<TContext> CurrentStep
        {
            get { return _currentStep; }
            private set
            {
                _currentStep = value;
                RaisePropertyChanged(() => CurrentStep);

                GoBackCommand.RaiseCanExecuteChanged();
                GoForwardCommand.RaiseCanExecuteChanged();
                FinishCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        public bool CanGoBack { get; private set; }

        public DelegateCommand GoBackCommand { get; private set; }

        public bool CanGoForward { get; private set; }

        public DelegateCommand GoForwardCommand { get; private set; }

        public DelegateCommand FinishCommand { get; private set; }

        public TContext Context
        {
            get { return _context; }
        }

        protected WizardViewModel(ILog log, ISchedulerProvider scheduler, IViewService viewService)
            : base(log, scheduler, viewService)
        {

            GoBackCommand = new DelegateCommand(() => GoBack(),
                                                () => CurrentStep.Ordinal > 0);

            GoForwardCommand = new DelegateCommand(() => GoForward(),
                                                   () => CurrentStep.Ordinal < _steps.Count - 1);

            FinishCommand = new DelegateCommand(() => Finish(),
                                                () => _canBeFinished);

            Context.CanBeFinishedChanged
                .TakeUntil(Closed)
                .Subscribe(x =>
                           {
                               _canBeFinished = true;
                               FinishCommand.RaiseCanExecuteChanged();
                           });
        }

        private void GoForward()
        {
            var index = CurrentStep.Ordinal;

            CurrentStep = _steps.Single(x => x.Key.Ordinal == index + 1).Value;

            var supportActivation = CurrentStep as ISupportActivationState;
            if (supportActivation != null)
            {
                supportActivation.Activate();
            }
        }

        private void GoBack()
        {
            var index = CurrentStep.Ordinal;

            CurrentStep = _steps.Single(x => x.Key.Ordinal == index - 1).Value;

            var supportActivation = CurrentStep as ISupportActivationState;
            if (supportActivation != null)
            {
                supportActivation.Activate();
            }
        }

        protected override Task OnInitialise()
        {
            return Task.Factory
                .StartNew(() =>
                          {
                              var steps = GetSteps();

                              var index = 0;
                              foreach (var step in steps)
                              {
                                  step.Context = Context;
                                  step.Ordinal = index;

                                  Disposables.Add(this.SyncViewModelActivationStates(step));
                                  Disposables.Add(BusyViewModel.SyncViewModelBusy(step.BusyViewModel));

                                  _steps.Add(new WizardStepIdentifier(step.Ordinal, step.Name), step);

                                  index++;
                              }

                              CurrentStep = _steps.First().Value;
                              GoBackCommand.RaiseCanExecuteChanged();
                              GoForwardCommand.RaiseCanExecuteChanged();
                          }, Scheduler.TPL.Task);
        }

        protected abstract IEnumerable<IWizardStepViewModel<TContext>> GetSteps();

        protected abstract void Finish();
    }
}