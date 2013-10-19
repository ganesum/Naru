using System.Reflection;

using Agatha.Common;
using Agatha.ServiceLayer;
using Agatha.Unity;

using Microsoft.Practices.Unity;

using Naru.Unity;

namespace Naru.Agatha
{
    public static class Bootstrapper
    {
        public static IUnityContainer ConfigureNaruAgathaClient(this IUnityContainer container, 
            Assembly requestResponseAssembly)
        {
            container.RegisterTransient<IRequestTask, RequestTask>();

            new ClientConfiguration(new Container(container))
                .AddRequestAndResponseAssembly(requestResponseAssembly)
                .Initialize();

            AgathaKnownTypeRegistration.RegisterWCFAgathaTypes(requestResponseAssembly);

            return container;
        }

        public static IUnityContainer ConfigureNaruAgathaServer(this IUnityContainer container, 
            Assembly requestResponseAssembly, Assembly handlerAssembly)
        {
            new ServiceLayerConfiguration(new Container(container))
                .AddRequestAndResponseAssembly(requestResponseAssembly)
                .AddRequestHandlerAssembly(handlerAssembly)
                .Initialize();

            AgathaKnownTypeRegistration.RegisterWCFAgathaTypes(requestResponseAssembly);

            return container;
        }
    }
}