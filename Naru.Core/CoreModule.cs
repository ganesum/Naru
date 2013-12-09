using Autofac;

namespace Naru.Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EventStream>().As<IEventStream>().SingleInstance();
        }
    }
}