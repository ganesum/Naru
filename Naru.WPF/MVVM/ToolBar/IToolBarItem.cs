namespace Naru.WPF.MVVM.ToolBar
{
    public interface IToolBarItem
    {
        string DisplayName { get; }

        bool IsVisible { get; set; }

        string ImageName { get; set; }
    }
}