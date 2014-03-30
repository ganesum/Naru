namespace Naru.Concurrency.Scheduler
{
    public interface ISchedulerProvider
    {
        ITaskScheduler Task { get; }

        IIOCompletionScheduler IOCompletion { get; }

        ICurrentScheduler Current { get; }

        IImmediateScheduler Immediate { get; }

        INewThreadScheduler NewThread { get; }

        IThreadPoolScheduler ThreadPool { get; }
    }
}