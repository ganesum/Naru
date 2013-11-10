using System;

namespace Naru.WPF.Scheduler
{
    public interface IDispatcherScheduler : ITPLScheduler, IRXScheduler
    {
        void ExecuteSync(Action action);
    }
}