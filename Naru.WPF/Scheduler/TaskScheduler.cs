using System.Reactive.Concurrency;

namespace Naru.WPF.Scheduler
{
    public class TaskScheduler : ITaskScheduler
    {
        public System.Threading.Tasks.TaskScheduler TPL { get; private set; }

        public IScheduler RX { get; private set; }

        public TaskScheduler()
        {
            TPL = System.Threading.Tasks.TaskScheduler.Default;
            RX = TaskPoolScheduler.Default;
        }
    }
}