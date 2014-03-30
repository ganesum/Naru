using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Naru.Concurrency.Scheduler;

namespace Naru.WPF.Scheduler
{
    public class DispatcherSchedulerProvider : SchedulerProvider, IDispatcherSchedulerProvider
    {
        public IDispatcherScheduler Dispatcher { get; private set; }

        public DispatcherSchedulerProvider()
        {
            Dispatcher = new DispatcherScheduler(Application.Current.Dispatcher);
        }
    }
}