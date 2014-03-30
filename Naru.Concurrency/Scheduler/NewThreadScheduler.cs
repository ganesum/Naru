using System.Reactive.Concurrency;

namespace Naru.Concurrency.Scheduler
{
    public class NewThreadScheduler : INewThreadScheduler
    {
        public IScheduler RX { get; private set; }

        public NewThreadScheduler()
        {
            RX = System.Reactive.Concurrency.NewThreadScheduler.Default;
        }
    }
}