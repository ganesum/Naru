using Naru.WPF.MVVM;

namespace Naru.WPF.Menu
{
    public interface IMenuService
    {
        BindableCollection<IMenuItem> Items { get; }

        MenuButtonItem CreateMenuButtonItem();

        MenuGroupItem CreateMenuGroupItem();

        MenuSeperatorItem CreateMenuSeperatorItem();
    }
}