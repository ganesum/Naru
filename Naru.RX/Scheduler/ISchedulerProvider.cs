﻿using System.Reactive.Concurrency;

namespace Naru.RX.Scheduler
{
    public interface ISchedulerProvider
    {
        IScheduler CurrentThread { get; }
        IScheduler Dispatcher { get; }
        IScheduler Immediate { get; }
        IScheduler NewThread { get; }
        IScheduler ThreadPool { get; }
        IScheduler TaskPool { get; }
    }
}