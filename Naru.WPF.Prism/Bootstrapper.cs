using Microsoft.Practices.Unity;

using Naru.WPF.Prism.Region;

namespace Naru.WPF.Prism
{
    public static class Bootstrapper
    {
        public static IUnityContainer ConfigureNaruPrism(this IUnityContainer container)
        {
            container
                .RegisterTransient<IRegionService, RegionService>()
                .RegisterTransient<IRegionBuilder, RegionBuilder>()
                .RegisterType(typeof(IRegionBuilder<>), typeof(RegionBuilder<>));

            return container;
        }
    }
}