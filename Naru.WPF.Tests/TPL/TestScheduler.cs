using System.Threading.Tasks;

using Naru.WPF.TPL;

namespace Naru.WPF.Tests.TPL
{
    public class TestScheduler : IScheduler
    {
        public TaskScheduler Default { get; private set; }

        public DispatcherTaskSchedulerBase Dispatcher { get; private set; }

        public TestScheduler()
        {
            Default = new CurrentThreadTaskScheduler();
            Dispatcher = new CurrentThreadTaskScheduler();
        }
    }
}