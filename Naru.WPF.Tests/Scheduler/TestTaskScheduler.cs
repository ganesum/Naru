using System.Reactive.Concurrency;

using Microsoft.Reactive.Testing;

using Naru.TPL;
using Naru.WPF.Scheduler;

using TaskScheduler = System.Threading.Tasks.TaskScheduler;

namespace Naru.WPF.Tests.Scheduler
{
    public class TestTaskScheduler : ITaskScheduler
    {
        public TaskScheduler TPL { get; private set; }

        public IScheduler RX { get; private set; }

        public TestTaskScheduler()
        {
            TPL = new CurrentThreadTaskScheduler();
            RX = new TestScheduler();
        }
    }
}