using Naru.WPF.MVVM;

namespace Naru.WPF.ToolBar
{
    public interface IToolBarService
    {
        BindableCollection<IToolBarItem> Items { get; }

        ToolBarButtonItem CreateToolBarButtonItem();
    }
}