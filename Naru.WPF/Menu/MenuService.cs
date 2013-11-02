using System;

using Naru.WPF.MVVM;

namespace Naru.WPF.Menu
{
    public class MenuService : IMenuService
    {
        private readonly Func<MenuButtonItem> _menuButtonItemFactory;
        private readonly Func<MenuGroupItem> _menuGroupItemFactory;
        private readonly Func<MenuSeperatorItem> _menuSeperatorItemFactory;

        public BindableCollection<IMenuItem> Items { get; private set; }

        public MenuService(BindableCollection<IMenuItem> itemsCollection,
                           Func<MenuButtonItem> menuButtonItemFactory,
                           Func<MenuGroupItem> menuGroupItemFactory,
                           Func<MenuSeperatorItem> menuSeperatorItemFactory)
        {
            _menuButtonItemFactory = menuButtonItemFactory;
            _menuGroupItemFactory = menuGroupItemFactory;
            _menuSeperatorItemFactory = menuSeperatorItemFactory;
            Items = itemsCollection;
        }

        public MenuButtonItem CreateMenuButtonItem()
        {
            return _menuButtonItemFactory();
        }

        public MenuGroupItem CreateMenuGroupItem()
        {
            return _menuGroupItemFactory();
        }

        public MenuSeperatorItem CreateMenuSeperatorItem()
        {
            return _menuSeperatorItemFactory();
        }
    }
}