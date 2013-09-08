namespace Naru.WPF.MVVM.ToolBar
{
    public interface IToolBarService
    {
        BindableCollection<IToolBarItem> Items { get; }

        ToolBarButtonItem CreateToolBarButtonItem();
    }
}