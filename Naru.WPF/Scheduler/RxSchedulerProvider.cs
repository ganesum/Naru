using System.Reactive.Concurrency;

namespace Naru.WPF.Scheduler
{
    public sealed class RxSchedulerProvider : IRXSchedulerProvider
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

        public RxSchedulerProvider()
        {
            Dispatcher = DispatcherScheduler.Current;
        }
    }
}