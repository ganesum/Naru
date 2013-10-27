using System.Windows;
using System.Windows.Controls;

namespace Naru.WPF.ContextMenu
{
    public class ContextMenuItemTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return null;

            switch (item.GetType().Name)
            {
                case "ContextMenuGroupItem":
                {
                    return Application.Current.TryFindResource("ContextMenuItemTemplate") as DataTemplate;
                }
                case "ContextMenuButtonItem":
                {
                    return Application.Current.TryFindResource("ContextMenuItemTemplate") as DataTemplate;
                }
                case "ContextMenuSeperatorItem":
                {
                    return Application.Current.TryFindResource("ContextMenuSeperatorItemTemplate") as DataTemplate;
                }
            }

            return null;
        }
    }
}