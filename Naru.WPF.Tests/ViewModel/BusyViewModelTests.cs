using System;

using Naru.Tests.UnityAutoMockContainer;
using Naru.WPF.ViewModel;

using NUnit.Framework;

namespace Naru.WPF.Tests.ViewModel
{
    [TestFixture]
    public class BusyViewModelTests
    {
        public class when_Active
        {
            [Test]
            public void is_called_then_IsActive_is_set_to_true()
            {
                var container = new UnityAutoMockContainer();

                var viewModel = container.Resolve<BusyViewModel>();

                Assert.That(viewModel.IsActive, Is.False);

                viewModel.Active(string.Empty);

                Assert.That(viewModel.IsActive, Is.True);
            }

            [Test]
            public void is_called_with_a_message_then_Message_is_set_to_the_message()
            {
                var container = new UnityAutoMockContainer();

                var viewModel = container.Resolve<BusyViewModel>();

                Assert.That(viewModel.Message, Is.Null);

                var busyMessage = Guid.NewGuid().ToString();

                viewModel.Active(busyMessage);

                Assert.That(viewModel.Message, Is.EqualTo(busyMessage));
            }
        }

        public class when_InActive
        {
            [Test]
            public void is_called_then_IsActive_is_set_to_false()
            {
                var container = new UnityAutoMockContainer();

                var viewModel = container.Resolve<BusyViewModel>();
                viewModel.Active(string.Empty);

                Assert.That(viewModel.IsActive, Is.True);

                viewModel.InActive();

                Assert.That(viewModel.IsActive, Is.False);
            }

            [Test]
            public void is_called_then_Message_is_set_to_empty_string()
            {
                var container = new UnityAutoMockContainer();

                var viewModel = container.Resolve<BusyViewModel>();

                var busyMessage = Guid.NewGuid().ToString();

                viewModel.Active(busyMessage);

                viewModel.InActive();

                Assert.That(viewModel.Message, Is.EqualTo(string.Empty));
            }
        }
    }
}