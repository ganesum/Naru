using System.Threading.Tasks;

using Naru.WPF.TPL;

namespace Naru.WPF.Tests.TPL
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