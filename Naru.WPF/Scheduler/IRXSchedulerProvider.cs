using System.Reactive.Concurrency;

namespace Naru.WPF.Scheduler
{
    public interface IRXSchedulerProvider
    {
        IScheduler CurrentThread { get; }

        IScheduler Dispatcher { get; }

        IScheduler Immediate { get; }

        IScheduler NewThread { get; }

        IScheduler ThreadPool { get; }

        IScheduler TaskPool { get; }
    }
}