using System.Threading.Tasks;

namespace Naru.WPF.Scheduler
{
    public interface IScheduler
    {
        TaskScheduler Task { get; }

        DispatcherTaskSchedulerBase Dispatcher { get; }
    }
}