namespace Naru.Concurrency.Scheduler
{
    public interface ITPLScheduler
    {
        System.Threading.Tasks.TaskScheduler TPL { get; }
    }
}