using System.Reactive.Concurrency;

using Microsoft.Reactive.Testing;

using Naru.WPF.Scheduler;

namespace Naru.WPF.Tests.Scheduler
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