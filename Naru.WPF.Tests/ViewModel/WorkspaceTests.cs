using System;
using System.Threading.Tasks;

using Common.Logging;

using Naru.Core;
using Naru.Tests.UnityAutoMockContainer;
using Naru.TPL;
using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.Tests.Scheduler;
using Naru.WPF.ViewModel;

using NUnit.Framework;

namespace Naru.WPF.Tests.ViewModel
{
    [TestFixture]
    public class WorkspaceTests
    {
        public class WorkspaceViewModel : Workspace
        {
            public WorkspaceViewModel(ILog log, ISchedulerProvider scheduler, IViewService viewService) 
                : base(log, scheduler, viewService)
            {
                Disposables.Add(AnonymousDisposable.Create(() => DisposablesWasDisposed.SafeInvoke(this)));
            }

            public event EventHandler CleanUpWasCalled;

            public event EventHandler DisposablesWasDisposed;

            public event EventHandler OnInitialiseWasCalled;

            public event EventHandler OnActivateWasCalled;

            public event EventHandler OnDeActivateWasCalled;

            protected override Task OnInitialise()
            {
                OnInitialiseWasCalled.SafeInvoke(this);

                return CompletedTask.Default;
            }

            protected override void OnActivate()
            {
                OnActivateWasCalled.SafeInvoke(this);
            }

            protected override void OnDeActivate()
            {
                OnDeActivateWasCalled.SafeInvoke(this);
            }

            protected override void CleanUp()
            {
                CleanUpWasCalled.SafeInvoke(this);
            }
        }

        [TestFixture]
        public class ISupportClosing
        {
            public class when_Close
            {
                [Test]
                public void is_called_then_OnClosed_event_is_fired()
                {
                    var container = new UnityAutoMockContainer();

                    var eventWasFired = false;

                    var viewModel = container.Resolve<WorkspaceViewModel>();

                    Assert.That(eventWasFired, Is.False);

                    viewModel.Closed += (s, e) => eventWasFired = true;

                    viewModel.Close();

                    Assert.That(eventWasFired, Is.True);
                }

                [Test]
                public void is_called_then_Disposables_are_disposed()
                {
                    var container = new UnityAutoMockContainer();

                    var eventWasFired = false;

                    var viewModel = container.Resolve<WorkspaceViewModel>();

                    Assert.That(eventWasFired, Is.False);

                    viewModel.DisposablesWasDisposed += (s, e) => eventWasFired = true;

                    viewModel.Close();

                    Assert.That(eventWasFired, Is.True);
                }

                [Test]
                public void is_called_then_CleanUp_is_called()
                {
                    var container = new UnityAutoMockContainer();

                    var eventWasFired = false;

                    var viewModel = container.Resolve<WorkspaceViewModel>();

                    Assert.That(eventWasFired, Is.False);

                    viewModel.CleanUpWasCalled += (s, e) => eventWasFired = true;

                    viewModel.Close();

                    Assert.That(eventWasFired, Is.True);
                }
            }

            [Test]
            public void when_CloseCommand_is_executed_then_OnClosed_event_is_fired()
            {
                var container = new UnityAutoMockContainer();

                var eventWasFired = false;

                var viewModel = container.Resolve<WorkspaceViewModel>();

                Assert.That(eventWasFired, Is.False);

                viewModel.Closed += (s, e) => eventWasFired = true;

                viewModel.CloseCommand.Execute(null);

                Assert.That(eventWasFired, Is.True);
            }
        }

        public class ISupportActivationStateTests
        {
            [TestFixture]
            public class when_Activate
            {
                [Test]
                public void is_called_multiple_time_then_OnInitialise_is_only_called_once()
                {
                    var container = new UnityAutoMockContainer();

                    var eventWasFired = 0;

                    container.RegisterInstance<ISchedulerProvider>(new TestSchedulerProvider());
                    var viewModel = container.Resolve<WorkspaceViewModel>();

                    Assert.That(eventWasFired, Is.EqualTo(0));

                    viewModel.OnInitialiseWasCalled += (s, e) => eventWasFired++;

                    ((ISupportActivationState)viewModel).Activate();
                    ((ISupportActivationState)viewModel).DeActivate();
                    ((ISupportActivationState)viewModel).Activate();

                    Assert.That(eventWasFired, Is.EqualTo(1));
                }

                [Test]
                public void is_called_multiple_time_then_OnInitialised_event_is_only_fired_once()
                {
                    var container = new UnityAutoMockContainer();

                    var eventWasFired = 0;

                    container.RegisterInstance<ISchedulerProvider>(new TestSchedulerProvider());
                    var viewModel = container.Resolve<WorkspaceViewModel>();

                    Assert.That(eventWasFired, Is.EqualTo(0));

                    viewModel.Initialised += (s, e) => eventWasFired++;

                    ((ISupportActivationState)viewModel).Activate();
                    ((ISupportActivationState)viewModel).DeActivate();
                    ((ISupportActivationState)viewModel).Activate();

                    Assert.That(eventWasFired, Is.EqualTo(1));
                }

                [Test]
                public void is_called_then_IsActive_is_true()
                {
                    var container = new UnityAutoMockContainer();

                    container.RegisterInstance<ISchedulerProvider>(new TestSchedulerProvider());
                    var viewModel = container.Resolve<WorkspaceViewModel>();

                    Assert.That(viewModel.IsActive, Is.False);

                    ((ISupportActivationState)viewModel).Activate();

                    Assert.That(viewModel.IsActive, Is.True);
                }

                [Test]
                public void is_called_then_OnActivationStateChanged_event_is_fired()
                {
                    var container = new UnityAutoMockContainer();

                    var eventWasFired = false;

                    container.RegisterInstance<ISchedulerProvider>(new TestSchedulerProvider());
                    var viewModel = container.Resolve<WorkspaceViewModel>();

                    Assert.That(eventWasFired, Is.False);

                    viewModel.ActivationStateChanged += (s, e) => eventWasFired = true;

                    ((ISupportActivationState)viewModel).Activate();

                    Assert.That(eventWasFired, Is.True);
                }

                [Test]
                public void is_called_then_OnActivate_is_called()
                {
                    var container = new UnityAutoMockContainer();

                    var eventWasFired = false;

                    container.RegisterInstance<ISchedulerProvider>(new TestSchedulerProvider());
                    var viewModel = container.Resolve<WorkspaceViewModel>();

                    Assert.That(eventWasFired, Is.False);

                    viewModel.OnActivateWasCalled += (s, e) => eventWasFired = true;

                    ((ISupportActivationState)viewModel).Activate();

                    Assert.That(eventWasFired, Is.True);
                }
            }

            [TestFixture]
            public class when_DeActivate
            {
                [Test]
                public void is_called_then_IsActive_is_false()
                {
                    var container = new UnityAutoMockContainer();

                    container.RegisterInstance<ISchedulerProvider>(new TestSchedulerProvider());
                    var viewModel = container.Resolve<WorkspaceViewModel>();

                    Assert.That(viewModel.IsActive, Is.False);

                    ((ISupportActivationState)viewModel).Activate();

                    Assert.That(viewModel.IsActive, Is.True);

                    ((ISupportActivationState)viewModel).DeActivate();

                    Assert.That(viewModel.IsActive, Is.False);
                }

                [Test]
                public void is_called_then_OnActivationStateChanged_event_is_fired()
                {
                    var container = new UnityAutoMockContainer();

                    var eventWasFired = false;

                    container.RegisterInstance<ISchedulerProvider>(new TestSchedulerProvider());
                    var viewModel = container.Resolve<WorkspaceViewModel>();

                    Assert.That(eventWasFired, Is.False);

                    viewModel.ActivationStateChanged += (s, e) => eventWasFired = true;

                    ((ISupportActivationState)viewModel).DeActivate();

                    Assert.That(eventWasFired, Is.True);
                }

                [Test]
                public void is_called_then_OnDeActivate_is_called()
                {
                    var container = new UnityAutoMockContainer();

                    var eventWasFired = false;

                    container.RegisterInstance<ISchedulerProvider>(new TestSchedulerProvider());
                    var viewModel = container.Resolve<WorkspaceViewModel>();

                    Assert.That(eventWasFired, Is.False);

                    viewModel.OnDeActivateWasCalled += (s, e) => eventWasFired = true;

                    ((ISupportActivationState)viewModel).DeActivate();

                    Assert.That(eventWasFired, Is.True);
                }
            }
        }
    }
}