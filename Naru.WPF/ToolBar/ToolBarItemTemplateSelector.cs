using System.Windows;
using System.Windows.Controls;

namespace Naru.WPF.ToolBar
{
    public class ToolBarItemTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return null;

            switch (item.GetType().Name)
            {
                case "ToolBarButtonItem":
                {
                    return Application.Current.TryFindResource("ToolBarButtonItemTemplate") as DataTemplate;
                }
            }

            return null;
        }
    }
}