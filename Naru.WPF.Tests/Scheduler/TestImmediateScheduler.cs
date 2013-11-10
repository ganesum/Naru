using System.Reactive.Concurrency;

using Microsoft.Reactive.Testing;

using Naru.WPF.Scheduler;

namespace Naru.WPF.Tests.Scheduler
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