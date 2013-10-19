using System.Windows;

namespace Naru.WPF.Scheduler
{
    public class SchedulerProvider : ISchedulerProvider
    {
        public ITPLSchedulerProvider TPL { get; private set; }

        public IRXSchedulerProvider RX { get; private set; }

        public IDispatcherService Dispatcher { get; private set; }

        public SchedulerProvider()
        {
            TPL = new TPLSchedulerProvider();
            RX = new RxSchedulerProvider();
            Dispatcher = new DispatcherService(Application.Current.Dispatcher);
        }
    }
}