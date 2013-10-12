using Naru.WPF.MVVM;

namespace Naru.WPF.Prism.Region
{
    public interface IRegionService
    {
        IRegionBuilder<TViewModel> RegionBuilder<TViewModel>()
            where TViewModel : IViewModel;

        IRegionBuilder RegionBuilder();
    }
}