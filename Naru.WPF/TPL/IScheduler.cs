using System.Threading.Tasks;

namespace Naru.WPF.TPL
{
    public interface IScheduler
    {
        TaskScheduler Default { get; }

        DispatcherTaskSchedulerBase Dispatcher { get; }
    }
}