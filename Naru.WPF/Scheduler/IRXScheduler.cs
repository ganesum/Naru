using System.Reactive.Concurrency;

namespace Naru.WPF.Scheduler
{
    public interface IRXScheduler
    {
        IScheduler RX { get; }
    }
}