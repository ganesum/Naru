using System;
using System.Windows;

using Naru.WPF.ViewModel;

namespace Naru.WPF.MVVM
{
    public class ViewServiceHelper
    {
        public static FrameworkElement CreateView(Type viewModelType)
        {
            // Work out the view type from the ViewModel type 
            var viewTypeName = viewModelType.FullName.Replace("Model", "");

            // Check to see if there is a UseViewAttribute on the ViewModel
            var useViewAttribute = Attribute.GetCustomAttribute(viewModelType, typeof (UseViewAttribute), true) as UseViewAttribute;
            var viewType = useViewAttribute != null ? useViewAttribute.ViewType : viewModelType.Assembly.GetType(viewTypeName);

            var view = (FrameworkElement)Activator.CreateInstance(viewType);

            return view;
        }

        public static void BindViewModel<TViewModel>(FrameworkElement view, TViewModel viewModel)
            where TViewModel : IViewModel
        {
            view.DataContext = viewModel;
        }
    }
}