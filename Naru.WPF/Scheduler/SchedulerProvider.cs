using System.Windows;

namespace Naru.WPF.Scheduler
{
    public class SchedulerProvider : ISchedulerProvider
    {
        public IDispatcherScheduler Dispatcher { get; private set; }

        public ITaskScheduler Task { get; private set; }

        public IIOCompletionScheduler IOCompletion { get; private set; }

        public ICurrentScheduler Current { get; private set; }

        public IImmediateScheduler Immediate { get; private set; }

        public INewThreadScheduler NewThread { get; private set; }

        public IThreadPoolScheduler ThreadPool { get; private set; }

        public SchedulerProvider()
        {
            Dispatcher = new DispatcherScheduler(Application.Current.Dispatcher);
            Task = new TaskScheduler();
            IOCompletion = new IOCompletionScheduler();
            Current = new CurrentScheduler();
            Immediate = new ImmediateScheduler();
            NewThread = new NewThreadScheduler();
            ThreadPool = new ThreadPoolScheduler();
        }
    }
}