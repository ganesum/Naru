using Naru.WPF.Presentation;
using Naru.WPF.MVVM;

namespace Naru.WPF.Windows.Controls
{
    public interface IWindowViewModel
    {
        BindableCollection<Link> TitleLinks { get; }
    }
}