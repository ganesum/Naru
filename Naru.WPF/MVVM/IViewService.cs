using System.Threading.Tasks;

using Naru.WPF.ViewModel;

namespace Naru.WPF.MVVM
{
    public interface IViewService
    {
        void ShowModal(IViewModel viewModel);

        Task ShowModalAsync(IViewModel viewModel);
    }
}