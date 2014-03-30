using System.Reactive.Concurrency;

namespace Naru.Concurrency.Scheduler
{
    public class CurrentScheduler : ICurrentScheduler
    {
        public IScheduler RX { get; private set; }

        public CurrentScheduler()
        {
            RX = CurrentThreadScheduler.Instance;
        }
    }
}