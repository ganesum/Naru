namespace Naru.WPF.Scheduler
{
    public interface ISchedulerProvider
    {
        ITPLSchedulerProvider TPL { get; }

        IRXSchedulerProvider RX { get; }

        IDispatcherService Dispatcher { get; }
    }
}