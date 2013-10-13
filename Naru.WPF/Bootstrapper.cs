using Microsoft.Practices.Unity;

using Naru.Unity;
using Naru.WPF.MVVM;
using Naru.WPF.MVVM.Dialog;
using Naru.WPF.MVVM.Menu;
using Naru.WPF.MVVM.ToolBar;
using Naru.WPF.Scheduler;

namespace Naru.WPF
{
    public static class Bootstrapper
    {
        public static IUnityContainer ConfigureNaruWPF(this IUnityContainer container)
        {
            container
                .RegisterSingleton<IScheduler, DesktopScheduler>()
                .RegisterTransient<IViewService, ViewService>()
                .RegisterType(typeof (IDialogBuilder<>), typeof (DialogBuilder<>))
                .RegisterTransient<IStandardDialogBuilder, StandardDialogBuilder>()
                .RegisterSingleton<IToolBarService, ToolBarService>()
                .RegisterSingleton<IMenuService, MenuService>();

            // This must be done here, so the correct Dispatcher is created
            container.Resolve<IScheduler>();

            return container;
        }
    }
}