using System.Threading.Tasks;

namespace Naru.WPF.Scheduler
{
    public interface ITPLSchedulerProvider
    {
        TaskScheduler Task { get; }

        TaskScheduler Dispatcher { get; }

        TaskScheduler IOCompletion { get; }
    }
}