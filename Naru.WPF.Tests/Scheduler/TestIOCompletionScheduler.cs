using Naru.TPL;
using Naru.WPF.Scheduler;

using TaskScheduler = System.Threading.Tasks.TaskScheduler;

namespace Naru.WPF.Tests.Scheduler
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