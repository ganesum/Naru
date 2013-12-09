using Common.Logging;

using Naru.Tests;
using Naru.WPF.Dialog;
using Naru.WPF.Scheduler;
using Naru.WPF.Tests.Scheduler;
using Naru.WPF.ViewModel;

using NUnit.Framework;

namespace Naru.WPF.Tests.ViewModel
{
    [TestFixture]
    public class ViewModelHelpersTests
    {
        public class WorkspaceViewModel : Workspace
        {
            public WorkspaceViewModel(ILog log, ISchedulerProvider scheduler, IStandardDialog standardDialog) 
                : base(log, scheduler, standardDialog)
            {
            }
        }

        [Test]
        public void when_parent_is_activated_child_is_activated()
        {
            var container = AutoMock.GetLoose();

            container.Provide<ISchedulerProvider>(new TestSchedulerProvider());

            var parent = container.Create<WorkspaceViewModel>() as ISupportActivationState;
            parent.ActivationStateViewModel.DeActivate();
            Assert.That(parent.ActivationStateViewModel.IsActive, Is.False);

            var child = container.Create<WorkspaceViewModel>() as ISupportActivationState;
            child.ActivationStateViewModel.DeActivate();
            Assert.That(child.ActivationStateViewModel.IsActive, Is.False);

            parent.SyncViewModelActivationStates(child);

            parent.ActivationStateViewModel.Activate();

            Assert.That(parent.ActivationStateViewModel.IsActive, Is.True);
            Assert.That(child.ActivationStateViewModel.IsActive, Is.True);
        }

        [Test]
        public void when_parent_is_deactivated_child_is_activated()
        {
            var container = AutoMock.GetLoose();

            container.Provide<ISchedulerProvider>(new TestSchedulerProvider());

            var parent = container.Create<WorkspaceViewModel>() as ISupportActivationState;
            parent.ActivationStateViewModel.Activate();
            Assert.That(parent.ActivationStateViewModel.IsActive, Is.True);

            var child = container.Create<WorkspaceViewModel>() as ISupportActivationState;
            child.ActivationStateViewModel.Activate();
            Assert.That(child.ActivationStateViewModel.IsActive, Is.True);

            parent.SyncViewModelActivationStates(child);

            parent.ActivationStateViewModel.DeActivate();

            Assert.That(parent.ActivationStateViewModel.IsActive, Is.False);
            Assert.That(child.ActivationStateViewModel.IsActive, Is.False);
        }
    }
}