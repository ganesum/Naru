﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

using Naru.WPF.ViewModel;

namespace Naru.WPF.MVVM
{
    /// <summary>
    /// ContentControl Behavior that will resolve the correct view 
    /// for the ViewModel on the DataContext.
    /// </summary>
    public class ContentControlViewResolverBehavior : Behavior<ContentControl>
    {
        protected override void OnAttached()
        {
            RefreshContent();

            AssociatedObject.DataContextChanged += AssociatedObject_DataContextChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.DataContextChanged -= AssociatedObject_DataContextChanged;
        }

        private void AssociatedObject_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            RefreshContent();
        }

        private void RefreshContent()
        {
            AssociatedObject.Content = null;

            var viewModel = AssociatedObject.DataContext as IViewModel;
            if (viewModel == null)
            {
                AssociatedObject.Content = AssociatedObject.DataContext;
            }
            else
            {
                var view = viewModel.GetViewAndBind();

                AssociatedObject.Content = view;
            }
        }
    }
}