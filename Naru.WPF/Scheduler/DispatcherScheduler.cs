using System;
using System.Reactive.Concurrency;
using System.Windows.Threading;

namespace Naru.WPF.Scheduler
{
    public class DispatcherScheduler : IDispatcherScheduler
    {
        private readonly Dispatcher _dispatcher;

        public System.Threading.Tasks.TaskScheduler TPL { get; private set; }

        public IScheduler RX { get; private set; }

        public void ExecuteSync(Action action)
        {
            if (_dispatcher.CheckAccess())
            {
                action();
                return;
            }

            _dispatcher.BeginInvoke(action);
        }

        public DispatcherScheduler(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            TPL = new DispatcherTaskScheduler(dispatcher);
            RX = System.Reactive.Concurrency.DispatcherScheduler.Current;
        }
    }
}