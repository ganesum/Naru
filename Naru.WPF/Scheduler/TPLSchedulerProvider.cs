using System.Threading.Tasks;
using System.Windows;

namespace Naru.WPF.Scheduler
{
    public class TPLSchedulerProvider : ITPLSchedulerProvider
    {
        public TaskScheduler Task { get; private set; }

        public TaskScheduler Dispatcher { get; private set; }

        public TPLSchedulerProvider()
        {
            Task = TaskScheduler.Default;

            Dispatcher = new DispatcherTaskScheduler(Application.Current.Dispatcher);
        }
    }
}