using System.Reactive.Concurrency;

using Microsoft.Reactive.Testing;

using Naru.Concurrency.Scheduler;

namespace Naru.Concurrency.Tests.Scheduler
{
    public class TestImmediateScheduler : IImmediateScheduler
    {
        public IScheduler RX { get; private set; }

        public TestImmediateScheduler()
        {
            RX = new TestScheduler();
        }
    }
}