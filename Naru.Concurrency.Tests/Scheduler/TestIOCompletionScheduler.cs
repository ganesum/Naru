using Naru.Concurrency.Scheduler;
using Naru.TPL;

using TaskScheduler = System.Threading.Tasks.TaskScheduler;

namespace Naru.Concurrency.Tests.Scheduler
{
    public class TestIOCompletionScheduler : IIOCompletionScheduler
    {
        public TaskScheduler TPL { get; private set; }

        public TestIOCompletionScheduler()
        {
            TPL = new CurrentThreadTaskScheduler();
        }
    }
}