using Naru.Concurrency.Scheduler;
using Naru.WPF.Scheduler;

namespace Naru.WPF
{
    public class WPFStartable
    {
        public WPFStartable(ISchedulerProvider schedulerProvider)
        {
            // ISchedulerProvider must be created here, so the correct Dispatcher is created
        }

        public void Start()
        {
        }
    }
}