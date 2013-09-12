using Microsoft.Practices.Unity;

using Naru.WPF.MVVM;
using Naru.WPF.MVVM.Dialog;
using Naru.WPF.MVVM.Menu;
using Naru.WPF.MVVM.Prism;
using Naru.WPF.MVVM.ToolBar;
using Naru.WPF.TPL;

namespace Naru.WPF
{
    public static class Bootstrapper
    {
        public static IUnityContainer ConfigureNaru(this IUnityContainer container)
        {
            container
                .RegisterSingleton<IScheduler, DesktopScheduler>()
                .RegisterTransient<IViewService, ViewService>()
                .RegisterType(typeof (IDialogBuilder<>), typeof (DialogBuilder<>))
                .RegisterTransient<IStandardDialogBuilder, StandardDialogBuilder>()
                .RegisterTransient<IRegionBuilder, RegionBuilder>()
                .RegisterType(typeof (IRegionBuilder<>), typeof (RegionBuilder<>))
                .RegisterSingleton<IToolBarService, ToolBarService>()
                .RegisterSingleton<IMenuService, MenuService>();

            // This must be done here, so the correct Dispatcher is created
            container.Resolve<IScheduler>();

            return container;
        }
    }
}