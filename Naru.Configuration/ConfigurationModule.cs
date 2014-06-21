using Autofac;

namespace Naru.Configuration
{
    public class ConfigurationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConfigurationService>().As<IConfigurationService>().SingleInstance();
        }
    }
}