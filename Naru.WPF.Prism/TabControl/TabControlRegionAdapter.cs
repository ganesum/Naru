using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;

using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;

namespace Naru.WPF.Prism.TabControl
{
    public class TabControlRegionAdapter : RegionAdapterBase<System.Windows.Controls.TabControl>
    {
        private readonly IScheduler _scheduler;

        public TabControlRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory, IScheduler scheduler)
            : base(regionBehaviorFactory)
        {
            _scheduler = scheduler;
        }

        protected override void Adapt(IRegion region, System.Windows.Controls.TabControl regionTarget)
        {
            region.Views.CollectionChanged += (s, e) => RegionCollectionChanged(regionTarget, e);

            regionTarget.SelectionChanged += (s, e) => TabControlSelectionChanged(e);
        }

        private void RegionCollectionChanged(System.Windows.Controls.TabControl regionTarget, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (FrameworkElement view in e.NewItems)
                {
                    var viewModel = view.DataContext as IViewModel;
                    if (viewModel == null) continue;

                    var tabItem = new TabItem {Content = view};

                    SetupHeader(viewModel, tabItem);

                    ConnectUpActivation(viewModel, regionTarget, tabItem);
                    ConnectUpClosing(viewModel, regionTarget, tabItem);
                    ConnectUpVisibility(viewModel, tabItem);

                    regionTarget.Items.Add(tabItem);
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var view in e.OldItems)
                {
                    TabItem tabItem = null;

                    foreach (TabItem item in regionTarget.Items)
                    {
                        if (item.Content == view)
                            tabItem = item;
                    }

                    if (tabItem != null)
                        regionTarget.Items.Remove(tabItem);
                }
            }
        }

        private static void TabControlSelectionChanged(SelectionChangedEventArgs e)
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

            viewModel.Activate();
        }

        private static void DeActivate(object item)
        {
            var view = item as FrameworkElement;
            if (view == null) return;

            var viewModel = view.DataContext as ISupportActivationState;
            if (viewModel == null) return;

            viewModel.DeActivate();
        }

        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
        }

        private void ConnectUpClosing(IViewModel viewModel, System.Windows.Controls.TabControl tabControl, TabItem tabItem)
        {
            var supportClosing = viewModel as ISupportClosing;
            if (supportClosing == null) return;

            // ViewModel is closed
            EventHandler supportClosingClosed = null;
            supportClosingClosed = (s, e) =>
            {
                _scheduler.Dispatcher.ExecuteSync(() => tabControl.Items.Remove(tabItem));

                if (supportClosingClosed != null)
                {
                    supportClosing.Closed -= supportClosingClosed;
                }
            };
            supportClosing.Closed += supportClosingClosed;
        }

        private void ConnectUpActivation(IViewModel viewModel, System.Windows.Controls.TabControl tabControl, TabItem tabItem)
        {
            var supportActivationState = viewModel as ISupportActivationState;
            if (supportActivationState == null) return;

            var supportClosing = viewModel as ISupportClosing;
            if (supportClosing == null) return;

            EventHandler<DataEventArgs<bool>> supportActivationStateActivationStateChanged = null;
            supportActivationStateActivationStateChanged = (s, e) =>
            {
                if (supportActivationState.IsActive)
                {
                    _scheduler.Dispatcher.ExecuteSync(() => tabControl.SelectedItem = tabItem);
                }
            };
            supportActivationState.ActivationStateChanged += supportActivationStateActivationStateChanged;

            EventHandler supportClosingClosed = null;
            supportClosingClosed = (s, e) =>
            {
                if (supportActivationStateActivationStateChanged != null)
                {
                    supportActivationState.ActivationStateChanged -= supportActivationStateActivationStateChanged;
                }

                if (supportClosingClosed != null)
                {
                    supportClosing.Closed -= supportClosingClosed;
                }
            };
            supportClosing.Closed += supportClosingClosed;
        }

        private void ConnectUpVisibility(IViewModel viewModel, TabItem tabItem)
        {
            var supportVisibility = viewModel as ISupportVisibility;
            if (supportVisibility == null) return;

            var supportClosing = viewModel as ISupportClosing;
            if (supportClosing == null) return;

            _scheduler.Dispatcher.ExecuteSync(() => tabItem.Visibility = supportVisibility.IsVisible
                    ? Visibility.Visible
                    : Visibility.Collapsed);

            EventHandler<DataEventArgs<bool>> supportVisibilityIsVisibleChanged = null;
            supportVisibilityIsVisibleChanged = (s, e) =>
                _scheduler.Dispatcher.ExecuteSync(() => tabItem.Visibility = supportVisibility.IsVisible
                    ? Visibility.Visible
                    : Visibility.Collapsed);
            supportVisibility.IsVisibleChanged += supportVisibilityIsVisibleChanged;

            EventHandler supportClosingClosed = null;
            supportClosingClosed = (s, e) =>
            {
                if (supportVisibilityIsVisibleChanged != null)
                {
                    supportVisibility.IsVisibleChanged -= supportVisibilityIsVisibleChanged;
                }

                if (supportClosingClosed != null)
                {
                    supportClosing.Closed -= supportClosingClosed;
                }
            };
            supportClosing.Closed += supportClosingClosed;
        }

        private static void SetupHeader(IViewModel viewModel, TabItem tabItem)
        {
            var supportHeader = viewModel as ISupportHeader;
            if (supportHeader == null) return;

            var headerViewModel = supportHeader.Header;
            if (headerViewModel == null) return;

            var tabControlHeaderViewModel = new TabControlHeaderViewModel(supportHeader);
            var tabControlHeaderView = ViewService.CreateView(tabControlHeaderViewModel.GetType());
            ViewService.BindViewModel(tabControlHeaderView, tabControlHeaderViewModel);

            tabItem.Header = tabControlHeaderView;
        }
    }
}