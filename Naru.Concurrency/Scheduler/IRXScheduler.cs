using System.Reactive.Concurrency;

namespace Naru.Concurrency.Scheduler
{
    public interface IRXScheduler
    {
        IScheduler RX { get; }
    }
}