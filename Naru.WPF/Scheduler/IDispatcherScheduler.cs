using System;

using Naru.Concurrency.Scheduler;

namespace Naru.WPF.Scheduler
{
    public interface IDispatcherScheduler : ITPLScheduler, IRXScheduler
    {
        void ExecuteSync(Action action);
    }
}