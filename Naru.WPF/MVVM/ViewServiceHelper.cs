using System;
using System.Windows;

using Naru.WPF.ViewModel;

namespace Naru.WPF.MVVM
{
    public static class ViewServiceHelper
    {
        public static object CreateView(Type viewModelType)
        {
            // Work out the view type from the ViewModel type 
            var viewTypeName = viewModelType.FullName.Replace("Model", string.Empty);

            // Check to see if there is a UseViewAttribute on the ViewModel
            var useViewAttribute = Attribute.GetCustomAttribute(viewModelType, typeof (UseViewAttribute), true) as UseViewAttribute;
            var viewType = useViewAttribute != null ? useViewAttribute.ViewType : viewModelType.Assembly.GetType(viewTypeName);

            return viewType != null ? Activator.CreateInstance(viewType) : viewModelType.FullName;
        }

        public static void BindViewModel<TViewModel>(object view, TViewModel viewModel)
            where TViewModel : IViewModel
        {
            var frameworkElement = view as FrameworkElement;
            if (frameworkElement != null)
            {
                frameworkElement.DataContext = viewModel;
            }
        }

        public static object GetViewAndBind<TViewModel>(this TViewModel viewModel)
            where TViewModel : IViewModel
        {
            var view = CreateView(viewModel.GetType());

            BindViewModel(view, viewModel);

            return view;
        }
    }
}