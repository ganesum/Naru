using System;

namespace Naru.WPF.Scheduler
{
    public interface IDispatcherService
    {
        void ExecuteSync(Action action);
    }
}