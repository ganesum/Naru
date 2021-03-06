﻿using System.Windows.Input;

using Naru.RX;
using Naru.WPF.Scheduler;
using Naru.WPF.ViewModel;

namespace Naru.WPF.ContextMenu
{
    public class ContextMenuButtonItem : ViewModel.ViewModel, IContextMenuItem
    {
        public string DisplayName { get; set; }

        #region IsVisible

        private readonly ObservableProperty<bool> _isVisible = new ObservableProperty<bool>();

        public bool IsVisible
        {
            get { return _isVisible.Value; }
            set { _isVisible.RaiseAndSetIfChanged(value); }
        }

        #endregion

        public ICommand Command { get; set; }

        public string ImageName { get; set; }

        public ContextMenuButtonItem(IDispatcherSchedulerProvider scheduler)
        {
            _isVisible.ConnectINPCProperty(this, () => IsVisible, scheduler).AddDisposable(Disposables);

            IsVisible = true;
        }
    }
}