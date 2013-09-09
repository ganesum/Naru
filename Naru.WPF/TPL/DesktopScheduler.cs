using System.Threading;
using System.Threading.Tasks;

namespace Naru.WPF.TPL
{
    public class DesktopScheduler : IScheduler
    {
        public TaskScheduler Task { get; private set; }

        public DispatcherTaskSchedulerBase Dispatcher { get; private set; }

        public DesktopScheduler()
        {
            Task = TaskScheduler.Default;
            Dispatcher = new SynchronizationContextTaskScheduler(SynchronizationContext.Current);
        }
    }
}