using Autofac;

using Naru.WPF.Dialog;
using Naru.WPF.Menu;
using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.ToolBar;
using Naru.WPF.ViewModel;

namespace Naru.WPF
{
    public class WPFModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SchedulerProvider>().As<ISchedulerProvider>();

            builder.RegisterType<ViewService>().As<IViewService>();

            // Dialogs
            builder.RegisterGeneric(typeof(DialogBuilder<>)).As(typeof(IDialogBuilder<>)).InstancePerDependency();
            builder.RegisterType<StandardDialogBuilder>().As<StandardDialogBuilder>().InstancePerDependency();

            // ToolBar
            builder.RegisterType<ToolBarService>().As<IToolBarService>().SingleInstance();
            builder.RegisterType<ToolBarButtonItem>().AsSelf().InstancePerDependency();

            // Menu
            builder.RegisterType<MenuService>().As<IMenuService>().SingleInstance();
            builder.RegisterType<MenuButtonItem>().AsSelf().InstancePerDependency();
            builder.RegisterType<MenuGroupItem>().AsSelf().InstancePerDependency();
            builder.RegisterType<MenuSeperatorItem>().AsSelf().InstancePerDependency();

            // Collections
            builder.RegisterGeneric(typeof(BindableCollection<>)).AsSelf().InstancePerDependency();
            builder.RegisterGeneric(typeof(ReactiveSingleSelectCollection<>)).AsSelf().InstancePerDependency();
            builder.RegisterGeneric(typeof(ReactiveMultiSelectCollection<>)).AsSelf().InstancePerDependency();
        }
    }
}