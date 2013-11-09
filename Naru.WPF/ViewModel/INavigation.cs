using Naru.WPF.Command;

namespace Naru.WPF.ViewModel
{
    public interface INavigation
    {
        bool CanGoBack { get; }

        DelegateCommand GoBackCommand { get; }

        bool CanGoForward { get; }

        DelegateCommand GoForwardCommand { get; }
    }
}