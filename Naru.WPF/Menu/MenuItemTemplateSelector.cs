using System.Windows;
using System.Windows.Controls;

namespace Naru.WPF.Menu
{
    public class MenuItemTemplateSelector : DataTemplateSelector
    {
        public const string MenuItemTemplate = "MenuItemTemplate";
        public const string MenuSeperatorItemTemplate = "MenuSeperatorItemTemplate";

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return null;

            if (item.GetType().Name == typeof (MenuGroupItem).Name)
            {
                return Application.Current.TryFindResource(MenuItemTemplate) as DataTemplate;
            }

            if (item.GetType().Name == typeof (MenuButtonItem).Name)
            {
                return Application.Current.TryFindResource(MenuItemTemplate) as DataTemplate;
            }

            if (item.GetType().Name == typeof (MenuSeperatorItem).Name)
            {
                return Application.Current.TryFindResource(MenuSeperatorItemTemplate) as DataTemplate;
            }

            return null;
        }
    }
}