using System;
using System.Windows;
using System.Windows.Interactivity;

using Naru.WPF.ViewModel;

namespace Naru.WPF.MVVM
{
    public class InitialisationBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = AssociatedObject.DataContext as IViewModel;
            if (viewModel == null) return;

            var supportActivationState = viewModel as ISupportActivationState;
            if (supportActivationState == null) return;

            supportActivationState.Activate();
        }
    }
}