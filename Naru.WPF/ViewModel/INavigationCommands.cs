using Naru.WPF.Command;

namespace Naru.WPF.ViewModel
{
    public interface INavigationCommands
    {
        DelegateCommand GoBackCommand { get; }

        DelegateCommand GoForwardCommand { get; }
    }
}