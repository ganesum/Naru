﻿using Naru.TPL;

namespace Naru.Concurrency.Scheduler
{
    public class IOCompletionScheduler : IIOCompletionScheduler
    {
        public System.Threading.Tasks.TaskScheduler TPL { get; private set; }

        public IOCompletionScheduler()
        {
            TPL = new IOCompletionPortTaskScheduler(50, 20);
        }
    }
}