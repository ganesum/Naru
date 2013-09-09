using System.Threading.Tasks;

namespace Naru.WPF.TPL
{
    public interface IScheduler
    {
        TaskScheduler Task { get; }

        DispatcherTaskSchedulerBase Dispatcher { get; }
    }
}