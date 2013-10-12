using Microsoft.Practices.Unity;

using Naru.WPF.MVVM;

namespace Naru.WPF.Prism.Region
{
    public class RegionService : IRegionService
    {
        private readonly IUnityContainer _container;

        public RegionService(IUnityContainer container)
        {
            _container = container;
        }

        public IRegionBuilder RegionBuilder()
        {
            return _container.Resolve<IRegionBuilder>();
        }

        public IRegionBuilder<TViewModel> RegionBuilder<TViewModel>()
            where TViewModel : IViewModel
        {
            return _container.Resolve<IRegionBuilder<TViewModel>>();
        }
    }
}