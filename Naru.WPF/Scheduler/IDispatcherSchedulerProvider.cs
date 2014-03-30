using Naru.Concurrency.Scheduler;

namespace Naru.WPF.Scheduler
{
    public interface IDispatcherSchedulerProvider : ISchedulerProvider
    {
        IDispatcherScheduler Dispatcher { get; }
    }
}