using System;

using Common.Logging.Simple;

using Naru.WPF.Tests.Scheduler;
using Naru.WPF.ViewModel;

using NUnit.Framework;

namespace Naru.WPF.Tests.ViewModel
{
    [TestFixture]
    public class ActivationStateViewModelTests
    {
        [Test]
        public void when_IsActive_is_set_to_true_then_ActivationStateChanged_pumps_true()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var activationStateViewModel = new ActivationStateViewModel(new NoOpLogger(), testSchedulerProvider);
            
            var result = false;

            activationStateViewModel.ActivationStateChanged
                                    .Subscribe(isActive => result = isActive);

            activationStateViewModel.Activate();

            Assert.That(result, Is.True);
        }

        [Test]
        public void when_IsActive_is_set_to_true_then_false_then_ActivationStateChanged_pumps_that_value()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var activationStateViewModel = new ActivationStateViewModel(new NoOpLogger(), testSchedulerProvider);

            activationStateViewModel.Activate();

            var result = true;

            activationStateViewModel.ActivationStateChanged
                                    .Subscribe(isActive => result = isActive);

            activationStateViewModel.DeActivate();

            Assert.That(result, Is.False);
        }

        [Test]
        public void the_first_time_IsActive_is_set_to_true_then_OnInitialise_pumps()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var activationStateViewModel = new ActivationStateViewModel(new NoOpLogger(), testSchedulerProvider);

            var result = false;

            activationStateViewModel.OnInitialise
                                    .Subscribe(_ => result = true);

            activationStateViewModel.Activate();

            Assert.That(result, Is.True);
        }

        [Test]
        public void the_second_time_IsActive_is_set_to_true_then_OnInitialise_does_not_pumps()
        {
            var testSchedulerProvider = new TestDispatcherSchedulerProvider();

            var activationStateViewModel = new ActivationStateViewModel(new NoOpLogger(), testSchedulerProvider);

            activationStateViewModel.Activate();
            activationStateViewModel.DeActivate();

            var result = false;

            activationStateViewModel.OnInitialise
                                    .Subscribe(_ => result = true);

            activationStateViewModel.Activate();

            Assert.That(result, Is.False);
        }
    }
}