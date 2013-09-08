using System.Threading.Tasks;

using Naru.WPF.MVVM.Dialog;

namespace Naru.WPF.MVVM
{
    public interface IViewService
    {
        IRegionBuilder<TViewModel> RegionBuilder<TViewModel>()
            where TViewModel : IViewModel;

        IRegionBuilder RegionBuilder();

        IDialogBuilder<Answer> DialogBuilder();

        IDialogBuilder<T> DialogBuilder<T>();

        void ShowModal(IViewModel viewModel);

        Task ShowModalAsync(IViewModel viewModel);

        IStandardDialogBuilder StandardDialogBuilder();
    }
}