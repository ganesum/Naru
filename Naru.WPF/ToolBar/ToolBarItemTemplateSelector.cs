using System.Windows;
using System.Windows.Controls;

namespace Naru.WPF.ToolBar
{
    public class ToolBarItemTemplateSelector : DataTemplateSelector
    {
        public const string ToolBarButtonItemTemplate = "ToolBarButtonItemTemplate";

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return null;

            if (item.GetType().Name == typeof (ToolBarButtonItem).Name)
            {
                return Application.Current.TryFindResource(ToolBarButtonItemTemplate) as DataTemplate;
            }

            return null;
        }
    }
}