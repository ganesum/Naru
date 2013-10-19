using System.Reactive.Concurrency;

using Microsoft.Reactive.Testing;

using Naru.WPF.Scheduler;

namespace Naru.WPF.Tests.Scheduler
{
    public sealed class TestRXSchedulerProvider : IRXSchedulerProvider
    {
        private readonly TestScheduler _currentThread = new TestScheduler();
        private readonly TestScheduler _dispatcher = new TestScheduler();
        private readonly TestScheduler _immediate = new TestScheduler();
        private readonly TestScheduler _newThread = new TestScheduler();
        private readonly TestScheduler _threadPool = new TestScheduler();
        private readonly TestScheduler _taskPool = new TestScheduler();

        #region Explicit implementation of ISchedulerService

        IScheduler IRXSchedulerProvider.CurrentThread
        {
            get { return _currentThread; }
        }

        IScheduler IRXSchedulerProvider.Dispatcher
        {
            get { return _dispatcher; }
        }

        IScheduler IRXSchedulerProvider.Immediate
        {
            get { return _immediate; }
        }

        IScheduler IRXSchedulerProvider.NewThread
        {
            get { return _newThread; }
        }

        IScheduler IRXSchedulerProvider.ThreadPool
        {
            get { return _threadPool; }
        }

        IScheduler IRXSchedulerProvider.TaskPool
        {
            get { return _taskPool; }
        }

        #endregion

        public TestScheduler CurrentThread
        {
            get { return _currentThread; }
        }

        public TestScheduler Dispatcher
        {
            get { return _dispatcher; }
        }

        public TestScheduler Immediate
        {
            get { return _immediate; }
        }

        public TestScheduler NewThread
        {
            get { return _newThread; }
        }

        public TestScheduler ThreadPool
        {
            get { return _threadPool; }
        }

        public TestScheduler TaskPool
        {
            get { return _taskPool; }
        }
    }
}