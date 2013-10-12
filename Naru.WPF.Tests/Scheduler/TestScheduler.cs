using System.Threading.Tasks;

using Naru.WPF.Scheduler;

namespace Naru.WPF.Tests.Scheduler
{
    public class TestScheduler : IScheduler
    {
        public TaskScheduler Task { get; private set; }

        public DispatcherTaskSchedulerBase Dispatcher { get; private set; }

        public TestScheduler()
        {
            Task = new CurrentThreadTaskScheduler();
            Dispatcher = new CurrentThreadTaskScheduler();
        }
    }
}