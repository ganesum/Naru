using System.Reactive.Concurrency;

using Microsoft.Reactive.Testing;

using Naru.WPF.Scheduler;

namespace Naru.WPF.Tests.Scheduler
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