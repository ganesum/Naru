using Naru.Concurrency.Tests.Scheduler;
using Naru.WPF.Scheduler;

namespace Naru.WPF.Tests.Scheduler
{
    public class TestDispatcherSchedulerProvider : TestSchedulerProvider, IDispatcherSchedulerProvider
    {
        public IDispatcherScheduler Dispatcher { get; private set; }

        public TestDispatcherSchedulerProvider()
        {
            Dispatcher = new TestDispatcherScheduler();
        }
    }
}