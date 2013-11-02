using Common.Logging;

using Naru.Tests;
using Naru.WPF.MVVM;
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
            public WorkspaceViewModel(ILog log, ISchedulerProvider scheduler, IViewService viewService) 
                : base(log, scheduler, viewService)
            {
            }
        }

        [Test]
        public void when_parent_is_activated_child_is_activated()
        {
            var container = AutoMock.GetLoose();

            container.Provide<ISchedulerProvider>(new TestSchedulerProvider());

            var parent = container.Create<WorkspaceViewModel>() as ISupportActivationState;
            parent.DeActivate();
            Assert.That(parent.IsActive, Is.False);

            var child = container.Create<WorkspaceViewModel>() as ISupportActivationState;
            child.DeActivate();
            Assert.That(child.IsActive, Is.False);

            parent.SyncViewModelActivationStates(child);

            parent.Activate();

            Assert.That(parent.IsActive, Is.True);
            Assert.That(child.IsActive, Is.True);
        }

        [Test]
        public void when_parent_is_deactivated_child_is_activated()
        {
            var container = AutoMock.GetLoose();

            container.Provide<ISchedulerProvider>(new TestSchedulerProvider());

            var parent = container.Create<WorkspaceViewModel>() as ISupportActivationState;
            parent.Activate();
            Assert.That(parent.IsActive, Is.True);

            var child = container.Create<WorkspaceViewModel>() as ISupportActivationState;
            child.Activate();
            Assert.That(child.IsActive, Is.True);

            parent.SyncViewModelActivationStates(child);

            parent.DeActivate();

            Assert.That(parent.IsActive, Is.False);
            Assert.That(child.IsActive, Is.False);
        }
    }
}