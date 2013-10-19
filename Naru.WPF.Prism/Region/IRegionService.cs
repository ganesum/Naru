using Naru.WPF.MVVM;
using Naru.WPF.ViewModel;

namespace Naru.WPF.Prism.Region
{
    public interface IRegionService
    {
        IRegionBuilder<TViewModel> RegionBuilder<TViewModel>()
            where TViewModel : IViewModel;

        IRegionBuilder RegionBuilder();
    }
}