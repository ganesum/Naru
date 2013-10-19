using System;

using Naru.WPF.Scheduler;

namespace Naru.WPF.Tests.Scheduler
{
    public class TestDispatcherService : IDispatcherService
    {
        public void ExecuteSync(Action action)
        {
            action();
        }
    }
}