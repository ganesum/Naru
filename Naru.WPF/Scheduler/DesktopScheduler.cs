using System.Threading.Tasks;
using System.Windows;

namespace Naru.WPF.Scheduler
{
    public class DesktopScheduler : IScheduler
    {
        public TaskScheduler Task { get; private set; }

        public DispatcherTaskSchedulerBase Dispatcher { get; private set; }

        public DesktopScheduler()
        {
            Task = TaskScheduler.Default;
            Dispatcher = new DispatcherTaskScheduler(Application.Current.Dispatcher);
            //Dispatcher = new SynchronizationContextTaskScheduler(SynchronizationContext.Current);
        }
    }
}