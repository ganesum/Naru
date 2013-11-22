using System;
using System.Reactive.Linq;

using Microsoft.Reactive.Testing;

using Naru.WPF.ViewModel;

using NUnit.Framework;

namespace Naru.WPF.Tests.ViewModel
{
    [TestFixture]
    public class BusyLatchTests
    {
        [Test]
        public void IsActive_is_true_when_subscription_is_made()
        {
            var testScheduler = new TestScheduler();

            var result = false;

            var busyLatch = new BusyLatch();

            busyLatch.IsActive
                     .ObserveOn(testScheduler)
                     .Subscribe(x => result = x);

            busyLatch
                .Subscribe(_ => { });

            testScheduler.AdvanceBy(1);

            Assert.That(result, Is.True);
        }

        [Test]
        public void IsActive_is_true_when_subscription_is_made_and_false_when_disposed()
        {
            var testScheduler = new TestScheduler();

            var result = false;

            var busyLatch = new BusyLatch();

            busyLatch.IsActive
                     .ObserveOn(testScheduler)
                     .Subscribe(x => result = x);

            var subscription = busyLatch
                .Subscribe(_ => { });

            testScheduler.AdvanceBy(1);

            Assert.That(result, Is.True);

            subscription.Dispose();

            testScheduler.AdvanceBy(1);

            Assert.That(result, Is.False);
        }

        [Test]
        public void when_used_with_TakeUntil_IsActive_is_true_then_false()
        {
            var testScheduler = new TestScheduler();

            var result = false;

            var busyLatch = new BusyLatch();

            busyLatch.IsActive
                     .ObserveOn(testScheduler)
                     .Subscribe(x => result = x);

            Observable.Range(0, 1)
                      .TakeUntil(busyLatch)
                      .Subscribe(x =>
                                 {

                                 });

            testScheduler.AdvanceBy(1);

            Assert.That(result, Is.True);

            testScheduler.AdvanceBy(1);

            Assert.That(result, Is.False);
        }

        [Test]
        public void when_used_with_TakeUntil_with_two_subscriptions_IsActive_is_true_true_true_then_false()
        {
            var testScheduler = new TestScheduler();

            var result = false;

            var busyLatch = new BusyLatch();

            busyLatch.IsActive
                     .ObserveOn(testScheduler)
                     .Subscribe(x => result = x);

            var observable = Observable.Range(0, 10, testScheduler);
            observable
                      .TakeUntil(busyLatch)
                      .Subscribe(x =>
                      {
                          System.Diagnostics.Debug.WriteLine("Sub1 - " + x);
                      });

            // To connect
            System.Diagnostics.Debug.WriteLine("AdvanceBy 1");
            testScheduler.AdvanceBy(1);

            Assert.That(result, Is.True);

            observable
                      .TakeUntil(busyLatch)
                      .Subscribe(x =>
                      {
                          System.Diagnostics.Debug.WriteLine("Sub2 - " + x);
                      });

            // To connect
            System.Diagnostics.Debug.WriteLine("AdvanceBy 1");
            testScheduler.AdvanceBy(1);

            Assert.That(result, Is.True);

            System.Diagnostics.Debug.WriteLine("AdvanceBy 10");
            testScheduler.AdvanceBy(10);

            Assert.That(result, Is.True);

            // To disconnect
            System.Diagnostics.Debug.WriteLine("AdvanceBy 1");
            testScheduler.AdvanceBy(1);

            // To disconnect
            System.Diagnostics.Debug.WriteLine("AdvanceBy 1");
            testScheduler.AdvanceBy(1);

            Assert.That(result, Is.False);
        }
    }
}