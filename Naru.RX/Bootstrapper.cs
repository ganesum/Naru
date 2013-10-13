using Microsoft.Practices.Unity;

using Naru.RX.Scheduler;
using Naru.Unity;

namespace Naru.RX
{
    public static class Bootstrapper
    {
        public static IUnityContainer ConfigureNaruRX(this IUnityContainer container)
        {
            container
                .RegisterSingleton<ISchedulerProvider, SchedulerProvider>();

            // This must be done here, so the correct Dispatcher is created
            container.Resolve<ISchedulerProvider>();

            return container;
        }
    }
}