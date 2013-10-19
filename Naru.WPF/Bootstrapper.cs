using Microsoft.Practices.Unity;

using Naru.Unity;
using Naru.WPF.Dialog;
using Naru.WPF.Menu;
using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.ToolBar;

namespace Naru.WPF
{
    public static class Bootstrapper
    {
        public static IUnityContainer ConfigureNaruWPF(this IUnityContainer container)
        {
            container
                .RegisterSingleton<ISchedulerProvider, SchedulerProvider>()
                .RegisterTransient<IViewService, ViewService>()
                .RegisterType(typeof (IDialogBuilder<>), typeof (DialogBuilder<>))
                .RegisterTransient<IStandardDialogBuilder, StandardDialogBuilder>()
                .RegisterSingleton<IToolBarService, ToolBarService>()
                .RegisterSingleton<IMenuService, MenuService>();

            // This must be done here, so the correct Dispatcher is created
            container.Resolve<ISchedulerProvider>();

            return container;
        }
    }
}