﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Common.Logging;

using Naru.RX;
using Naru.TPL;
using Naru.WPF.Command;
using Naru.WPF.Dialog;
using Naru.WPF.Scheduler;
using Naru.WPF.ViewModel;

namespace Naru.WPF.Wizard
{
    public abstract class WizardViewModel<TContext> : Workspace, INavigationCommands
        where TContext : IWizardContext, new()
    {
        private readonly TContext _context = new TContext();
        private readonly List<IWizardStepViewModel<TContext>> _steps = new List<IWizardStepViewModel<TContext>>();

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

                CurrentStep.Context = Context;

                GoBackCommand.RaiseCanExecuteChanged();
                GoForwardCommand.RaiseCanExecuteChanged();
                FinishCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        public DelegateCommand GoBackCommand { get; private set; }

        public DelegateCommand GoForwardCommand { get; private set; }

        public DelegateCommand FinishCommand { get; private set; }

        public TContext Context
        {
            get { return _context; }
        }

        protected WizardViewModel(ILog log, IDispatcherSchedulerProvider scheduler, IStandardDialog standardDialog)
            : base(log, scheduler, standardDialog)
        {

            GoBackCommand = new DelegateCommand(() => GoBack(),
                                                () => CurrentStep.Ordinal > 0);

            GoForwardCommand = new DelegateCommand(() => GoForward(),
                                                   () => CurrentStep.Ordinal < _steps.Count - 1);

            FinishCommand = new DelegateCommand(() => Finish(),
                                                () => _canBeFinished);

            Context.CanBeFinishedChanged
                   .TakeUntil(ClosingStrategy.Closed)
                   .Subscribe(x =>
                              {
                                  _canBeFinished = true;
                                  FinishCommand.RaiseCanExecuteChanged();
                              });
        }

        private void GoForward()
        {
            var index = CurrentStep.Ordinal;

            CurrentStep = _steps.Single(x => x.Ordinal == index + 1);

            var supportActivation = CurrentStep as ISupportActivationState;
            if (supportActivation != null)
            {
                supportActivation.ActivationStateViewModel.Activate();
            }
        }

        private void GoBack()
        {
            var index = CurrentStep.Ordinal;

            CurrentStep = _steps.Single(x => x.Ordinal == index - 1);

            var supportActivation = CurrentStep as ISupportActivationState;
            if (supportActivation != null)
            {
                supportActivation.ActivationStateViewModel.Activate();
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

                                  this.SyncViewModelActivationStates(step).AddDisposable(Disposables);
                                  this.SyncViewModelBusy(step).AddDisposable(Disposables);

                                  _steps.Add(step);

                                  index++;
                              }

                              CurrentStep = _steps.First();

                              GoBackCommand.RaiseCanExecuteChanged();
                              GoForwardCommand.RaiseCanExecuteChanged();
                          }, Scheduler.Task.TPL);
        }

        protected abstract IEnumerable<IWizardStepViewModel<TContext>> GetSteps();

        protected abstract void Finish();
    }
}