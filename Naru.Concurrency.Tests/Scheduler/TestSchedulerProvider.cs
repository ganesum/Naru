using Naru.Concurrency.Scheduler;

namespace Naru.Concurrency.Tests.Scheduler
{
    public class TestSchedulerProvider : ISchedulerProvider
    {
        public ITaskScheduler Task { get; private set; }

        public IIOCompletionScheduler IOCompletion { get; private set; }

        public ICurrentScheduler Current { get; private set; }

        public IImmediateScheduler Immediate { get; private set; }

        public INewThreadScheduler NewThread { get; private set; }

        public IThreadPoolScheduler ThreadPool { get; private set; }

        public TestSchedulerProvider()
        {
            Task = new TestTaskScheduler();
            IOCompletion = new TestIOCompletionScheduler();
            Current = new TestCurrentScheduler();
            Immediate = new TestImmediateScheduler();
            NewThread = new TestNewThreadScheduler();
            ThreadPool = new TestThreadPoolScheduler();
        }
    }
}