using Common.Logging;

using Moq;

using Naru.WPF.MVVM;
using Naru.WPF.TPL;

using NUnit.Framework;

namespace Naru.WPF.Tests.MVVM
{
    [TestFixture]
    public class ViewModelHelpersTests
    {
        public class StubISupportActivationState : Workspace
        {
            public StubISupportActivationState(ILog log, IScheduler scheduler) 
                : base(log, scheduler)
            {
            }
        }

        [Test]
        public void when_parent_is_activated_child_is_activated()
        {
            var parent = new StubISupportActivationState(new Mock<ILog>().Object, new Mock<IScheduler>().Object) as ISupportActivationState;
            parent.DeActivate();
            Assert.That(parent.IsActive, Is.False);

            var child = new StubISupportActivationState(new Mock<ILog>().Object, new Mock<IScheduler>().Object) as ISupportActivationState;
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
            var parent = new StubISupportActivationState(new Mock<ILog>().Object, new Mock<IScheduler>().Object) as ISupportActivationState;
            parent.Activate();
            Assert.That(parent.IsActive, Is.True);

            var child = new StubISupportActivationState(new Mock<ILog>().Object, new Mock<IScheduler>().Object) as ISupportActivationState;
            child.Activate();
            Assert.That(child.IsActive, Is.True);

            parent.SyncViewModelActivationStates(child);

            parent.DeActivate();

            Assert.That(parent.IsActive, Is.False);
            Assert.That(child.IsActive, Is.False);
        }
    }
}