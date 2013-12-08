using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using Microsoft.Reactive.Testing;

using NUnit.Framework;

namespace Naru.RX.Tests
{
    [TestFixture]
    public class ObservableExTests
    {
        [Test]
        public void WhenAny()
        {
            var testScheduler = new TestScheduler();

            var subject1 = new Subject<int>();
            var subject2 = new Subject<int>();

            var result = false;

            var disposable = ObservableEx.WhenAny(subject1, subject2)
                                         .ObserveOn(testScheduler)
                                         .Subscribe(x => result = true);

            subject1.OnNext(1);
            subject2.OnNext(2);

            testScheduler.AdvanceBy(1);

            disposable.Dispose();

            Assert.That(result, Is.True);
        }
    }
}