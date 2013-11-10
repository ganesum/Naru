namespace Naru.WPF.Scheduler
{
    public interface ISchedulerProvider
    {
        IDispatcherScheduler Dispatcher { get; }

        ITaskScheduler Task { get; }

        IIOCompletionScheduler IOCompletion { get; }

        ICurrentScheduler Current { get; }

        IImmediateScheduler Immediate { get; }

        INewThreadScheduler NewThread { get; }

        IThreadPoolScheduler ThreadPool { get; }
    }
}