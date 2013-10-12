using System;
using System.Threading.Tasks;

namespace Naru.WPF.Scheduler
{
    public abstract class DispatcherTaskSchedulerBase : TaskScheduler
    {
        public abstract void ExecuteSync(Action action);
    }
}