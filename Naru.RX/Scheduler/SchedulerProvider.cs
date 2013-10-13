using System.Reactive.Concurrency;

namespace Naru.RX.Scheduler
{
    public sealed class SchedulerProvider : ISchedulerProvider
    {
        public IScheduler CurrentThread
        {
            get { return System.Reactive.Concurrency.Scheduler.CurrentThread; }
        }

        public IScheduler Dispatcher { get; private set; }

        public IScheduler Immediate
        {
            get { return System.Reactive.Concurrency.Scheduler.Immediate; }
        }

        public IScheduler NewThread
        {
            get { return NewThreadScheduler.Default; }
        }

        public IScheduler ThreadPool
        {
            get { return ThreadPoolScheduler.Instance; }
        }

        public IScheduler TaskPool
        {
            get { return TaskPoolScheduler.Default; }
        }

        public SchedulerProvider()
        {
            Dispatcher = DispatcherScheduler.Current;
        }
    }
}