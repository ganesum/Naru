using Autofac;

namespace Naru.Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MessageStream>().As<IMessageStream>().SingleInstance();
        }
    }
}