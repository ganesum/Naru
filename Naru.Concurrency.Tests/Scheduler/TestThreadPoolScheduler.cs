using System.Reactive.Concurrency;

using Microsoft.Reactive.Testing;

using Naru.Concurrency.Scheduler;

namespace Naru.Concurrency.Tests.Scheduler
{
    public class TestThreadPoolScheduler : IThreadPoolScheduler
    {
        public IScheduler RX { get; private set; }

        public TestThreadPoolScheduler()
        {
            RX = new TestScheduler();
        }
    }
}