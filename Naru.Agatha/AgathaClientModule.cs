using System;
using System.Reflection;

using Agatha.Common;

using Autofac;

using Naru.Aufofac.Agatha;

using Module = Autofac.Module;

namespace Naru.Agatha
{
    public class AgathaClientModule : Module
    {
        public Assembly RequestResponseAssembly { get; set; }

        public Func<IContainer> ContainerFactory { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RequestTask>().As<IRequestTask>().InstancePerDependency();

            new ClientConfiguration(new Container(builder, ContainerFactory))
                .AddRequestAndResponseAssembly(RequestResponseAssembly)
                .Initialize();

            AgathaKnownTypeRegistration.RegisterWCFAgathaTypes(RequestResponseAssembly);
        }
    }
}