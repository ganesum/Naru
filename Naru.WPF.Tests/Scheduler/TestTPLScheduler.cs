using System.Threading.Tasks;

using Naru.WPF.Scheduler;

namespace Naru.WPF.Tests.Scheduler
{
    public class TestTPLScheduler : ITPLSchedulerProvider
    {
        public TaskScheduler Task { get; private set; }

        public TaskScheduler Dispatcher { get; private set; }

        public TestTPLScheduler()
        {
            Task = new CurrentThreadTaskScheduler();
            Dispatcher = new CurrentThreadTaskScheduler();
        }
    }
}