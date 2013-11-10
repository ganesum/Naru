using System.Threading.Tasks;
using System.Windows;

using Naru.TPL;

namespace Naru.WPF.Scheduler
{
    public class TPLSchedulerProvider : ITPLSchedulerProvider
    {
        public TaskScheduler Task { get; private set; }

        public TaskScheduler Dispatcher { get; private set; }
        public TaskScheduler IOCompletion { get; private set; }

        public TPLSchedulerProvider()
        {
            Task = TaskScheduler.Default;

            Dispatcher = new DispatcherTaskScheduler(Application.Current.Dispatcher);

            IOCompletion = new IOCompletionPortTaskScheduler(30, 50);
        }
    }
}