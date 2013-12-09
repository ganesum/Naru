using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Threading;

using Naru.WPF.ViewModel;

namespace Naru.WPF.TabControl
{
    public class TabControlItemSourceBehavior : Behavior<System.Windows.Controls.TabControl>
    {
        private Dispatcher _dispatcher;

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof (ObservableCollection<IViewModel>), 
            typeof (TabControlItemSourceBehavior), 
            new PropertyMetadata(default(ObservableCollection<IViewModel>), ItemsSourcePropertyChangedCallback));

        private static void ItemsSourcePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var behavior = dependencyObject as TabControlItemSourceBehavior;
            if (behavior == null)
            {
                return;
            }

            behavior.ItemsSource.CollectionChanged += behavior.items_CollectionChanged;

            foreach (var viewModel in behavior.ItemsSource)
            {
                behavior.AddViewModel(viewModel);
            }
        }

        public ObservableCollection<IViewModel> ItemsSource
        {
            get { return (ObservableCollection<IViewModel>) GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        protected override void OnAttached()
        {
            AssociatedObject.SelectionChanged += TabControlSelectionChanged;

            _dispatcher = Application.Current.Dispatcher;
        }

        protected override void OnDetaching()
        {
            ItemsSource.CollectionChanged -= items_CollectionChanged;

            AssociatedObject.SelectionChanged -= TabControlSelectionChanged;
        }

        private void items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (IViewModel viewModel in e.NewItems)
                {
                    if (AddViewModel(viewModel)) return;
                    else continue;
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var viewModel in e.OldItems)
                {
                    RemoveViewModel(viewModel);
                }
            }
        }

        private bool AddViewModel(IViewModel viewModel)
        {
            if (viewModel == null) return true;

            var view = viewModel.GetViewAndBind();

            var tabItem = new TabItem {Content = view};

            SetupHeader(viewModel, tabItem);

            ConnectUpActivation(viewModel, AssociatedObject, tabItem);
            ConnectUpClosing(viewModel, AssociatedObject, tabItem);

            AssociatedObject.Items.Add(tabItem);

            // If it is currently active, sync with the TabControl
            var supportActivationState = viewModel as ISupportActivationState;
            if (supportActivationState == null) return true;

            if (supportActivationState.ActivationStateViewModel.IsActive)
            {
                Action action = () => AssociatedObject.SelectedItem = tabItem;
                _dispatcher.BeginInvoke(action);
            }
            return false;
        }

        private void RemoveViewModel(object viewModel)
        {
            TabItem tabItem = null;

            foreach (TabItem item in AssociatedObject.Items)
            {
                if (viewModel == null) continue;

                var view = item.Content as FrameworkElement;
                if (view == null) continue;

                if (view.DataContext == viewModel)
                {
                    tabItem = item;
                }
            }

            if (tabItem != null)
            {
                AssociatedObject.Items.Remove(tabItem);
            }
        }

        private static void TabControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var obj in e.AddedItems)
            {
                var tabItem = obj as TabItem;
                if (tabItem == null) continue;

                var view = tabItem.Content;
                Activate(view);
                break;
            }

            foreach (var obj in e.RemovedItems)
            {
                var tabItem = obj as TabItem;
                if (tabItem == null) continue;

                var view = tabItem.Content;
                DeActivate(view);
            }
        }

        private static void Activate(object item)
        {
            var view = item as FrameworkElement;
            if (view == null) return;

            var viewModel = view.DataContext as ISupportActivationState;
            if (viewModel == null) return;

            viewModel.ActivationStateViewModel.Activate();
        }

        private static void DeActivate(object item)
        {
            var view = item as FrameworkElement;
            if (view == null) return;

            var viewModel = view.DataContext as ISupportActivationState;
            if (viewModel == null) return;

            viewModel.ActivationStateViewModel.DeActivate();
        }

        private void ConnectUpClosing(IViewModel viewModel, System.Windows.Controls.TabControl tabControl, TabItem tabItem)
        {
            var supportClosing = viewModel as ISupportClosing;
            if (supportClosing == null) return;

            // ViewModel is closed
            Action action = () => tabControl.Items.Remove(tabItem);
            supportClosing.ExecuteOnClosed(() => _dispatcher.BeginInvoke(action));
        }

        private void ConnectUpActivation(IViewModel viewModel, System.Windows.Controls.TabControl tabControl, TabItem tabItem)
        {
            var supportActivationState = viewModel as ISupportActivationState;
            if (supportActivationState == null) return;

            var supportClosing = viewModel as ISupportClosing;
            if (supportClosing == null) return;

            supportActivationState.ActivationStateViewModel.ActivationStateChanged
                                  .TakeUntil(supportClosing.Closed)
                                  .Subscribe(x =>
                                  {
                                      if (supportActivationState.ActivationStateViewModel.IsActive)
                                      {
                                          Action action = () => tabControl.SelectedItem = tabItem;
                                          _dispatcher.BeginInvoke(action);
                                      }
                                  });
        }

        private static void SetupHeader(IViewModel viewModel, TabItem tabItem)
        {
            var supportHeader = viewModel as ISupportHeader;
            if (supportHeader == null) return;

            var headerViewModel = supportHeader.Header;
            if (headerViewModel == null) return;

            var tabControlHeaderViewModel = new TabControlHeaderViewModel(supportHeader);
            var tabControlHeaderView = tabControlHeaderViewModel.GetViewAndBind();

            tabItem.Header = tabControlHeaderView;
        }
    }
}