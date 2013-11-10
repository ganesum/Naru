using System.Reactive.Concurrency;

using Microsoft.Reactive.Testing;

using Naru.WPF.Scheduler;

namespace Naru.WPF.Tests.Scheduler
{
    public class TestCurrentScheduler : ICurrentScheduler
    {
        public IScheduler RX { get; private set; }

        public TestCurrentScheduler()
        {
            RX = new TestScheduler();
        }
    }
}