using Naru.WPF.Scheduler;

namespace Naru.WPF.Tests.Scheduler
{
    public class TestSchedulerProvider : ISchedulerProvider
    {
        public ITPLSchedulerProvider TPL { get; private set; }

        public IRXSchedulerProvider RX { get; private set; }

        public IDispatcherService Dispatcher { get; private set; }

        public TestSchedulerProvider()
        {
            TPL = new TestTPLScheduler();

            RX = new TestRXSchedulerProvider();

            Dispatcher = new TestDispatcherService();
        }
    }
}