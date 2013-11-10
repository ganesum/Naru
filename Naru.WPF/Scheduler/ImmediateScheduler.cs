using System.Reactive.Concurrency;

namespace Naru.WPF.Scheduler
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