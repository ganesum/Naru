using System;

using Naru.Tests;
using Naru.WPF.Scheduler;
using Naru.WPF.Tests.Scheduler;
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
                var container = AutoMock.GetStrict();

                container.Provide<ISchedulerProvider>(new TestSchedulerProvider());

                var viewModel = container.Create<BusyViewModel>();

                Assert.That(viewModel.IsActive, Is.False);

                viewModel.Active(string.Empty);

                Assert.That(viewModel.IsActive, Is.True);
            }

            [Test]
            public void is_called_with_a_message_then_Message_is_set_to_the_message()
            {
                var container = AutoMock.GetStrict();

                container.Provide<ISchedulerProvider>(new TestSchedulerProvider());

                var viewModel = container.Create<BusyViewModel>();

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
                var container = AutoMock.GetStrict();

                container.Provide<ISchedulerProvider>(new TestSchedulerProvider());

                var viewModel = container.Create<BusyViewModel>();
                viewModel.Active(string.Empty);

                Assert.That(viewModel.IsActive, Is.True);

                viewModel.InActive();

                Assert.That(viewModel.IsActive, Is.False);
            }

            [Test]
            public void is_called_then_Message_is_set_to_empty_string()
            {
                var container = AutoMock.GetStrict();

                container.Provide<ISchedulerProvider>(new TestSchedulerProvider());

                var viewModel = container.Create<BusyViewModel>();

                var busyMessage = Guid.NewGuid().ToString();

                viewModel.Active(busyMessage);

                viewModel.InActive();

                Assert.That(viewModel.Message, Is.EqualTo(string.Empty));
            }
        }
    }
}