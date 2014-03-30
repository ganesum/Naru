using System.Reactive.Concurrency;

using Microsoft.Reactive.Testing;

using Naru.Concurrency.Scheduler;

namespace Naru.Concurrency.Tests.Scheduler
{
    public class TestNewThreadScheduler : INewThreadScheduler
    {
        public IScheduler RX { get; private set; }

        public TestNewThreadScheduler()
        {
            RX = new TestScheduler();
        }
    }
}