using System.Reactive.Concurrency;

namespace Naru.WPF.Scheduler
{
    public class ThreadPoolScheduler : IThreadPoolScheduler
    {
        public IScheduler RX { get; private set; }

        public ThreadPoolScheduler()
        {
            RX = System.Reactive.Concurrency.ThreadPoolScheduler.Instance;
        }
    }
}