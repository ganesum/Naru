using Autofac;
using Autofac.Builder;

using Naru.WPF.Dialog;
using Naru.WPF.Menu;
using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.ToolBar;
using Naru.WPF.UserInteractionHost;
using Naru.WPF.Validation;
using Naru.WPF.ViewModel;

namespace Naru.WPF
{
    public class WPFModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SchedulerProvider>().As<ISchedulerProvider>().SingleInstance();

            builder.RegisterType<ViewService>().As<IViewService>();

            // Dialogs
            builder.RegisterGeneric(typeof(DialogBuilder<>)).As(typeof(IDialogBuilder<>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(DialogViewModel<>)).AsSelf().InstancePerDependency();
            builder.RegisterType<StandardDialog>().As<IStandardDialog>().InstancePerDependency();

            // UserInteraction
            builder.RegisterType<UserInteraction>().As<IUserInteraction>().SingleInstance();
            builder.RegisterType<UserInteractionHostViewModel>().As<IUserInteractionHostViewModel>().InstancePerDependency();

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

            // Validation
            builder.RegisterGeneric(typeof (ValidationAsync<,>)).AsSelf().InstancePerDependency();
        }
    }
}