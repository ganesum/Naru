using System.Windows;
using System.Windows.Controls;

namespace Naru.WPF.ContextMenu
{
    public class ContextMenuItemTemplateSelector : DataTemplateSelector
    {
        public const string ContextMenuItemTemplate = "ContextMenuItemTemplate";
        public const string ContextMenuSeperatorItemTemplate = "ContextMenuSeperatorItemTemplate";

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return null;

            if (item.GetType().Name == typeof (ContextMenuGroupItem).Name)
            {
                return Application.Current.TryFindResource(ContextMenuItemTemplate) as DataTemplate;
            }

            if (item.GetType().Name == typeof (ContextMenuButtonItem).Name)
            {
                return Application.Current.TryFindResource(ContextMenuItemTemplate) as DataTemplate;
            }

            if (item.GetType().Name == typeof (ContextMenuSeperatorItem).Name)
            {

                return Application.Current.TryFindResource(ContextMenuSeperatorItemTemplate) as DataTemplate;
            }

            return null;
        }
    }
}