namespace Naru.WPF.Scheduler
{
    public interface ITPLScheduler
    {
        System.Threading.Tasks.TaskScheduler TPL { get; }
    }
}