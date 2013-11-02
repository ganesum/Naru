using System;
using System.Reflection;

using Agatha.ServiceLayer;
using Autofac;

using Naru.Aufofac.Agatha;

using Module = Autofac.Module;

namespace Naru.Agatha
{
    public class AgathaServerModule : Module
    {
        public Assembly RequestResponseAssembly { get; set; }

        public Assembly HandlerAssembly { get; set; }

        public Func<IContainer> ContainerFactory { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            new ServiceLayerConfiguration(new Container(builder, ContainerFactory))
                .AddRequestAndResponseAssembly(RequestResponseAssembly)
                .AddRequestHandlerAssembly(HandlerAssembly)
                .Initialize();

            AgathaKnownTypeRegistration.RegisterWCFAgathaTypes(RequestResponseAssembly);
        }
    }
}