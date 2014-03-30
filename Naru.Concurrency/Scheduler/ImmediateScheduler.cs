using System.Reactive.Concurrency;

namespace Naru.Concurrency.Scheduler
{
    public class ImmediateScheduler : IImmediateScheduler
    {
        public IScheduler RX { get; private set; }

        public ImmediateScheduler()
        {
            RX = System.Reactive.Concurrency.ImmediateScheduler.Instance;
        }
    }
}