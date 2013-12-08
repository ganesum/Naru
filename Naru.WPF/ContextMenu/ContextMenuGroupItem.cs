using Naru.RX;
using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.ViewModel;

namespace Naru.WPF.ContextMenu
{
    public class ContextMenuGroupItem : ViewModel.ViewModel, IContextMenuItem
    {
        public string DisplayName { get; set; }

        #region IsVisible

        private readonly ObservableProperty<bool> _isVisible = new ObservableProperty<bool>();

        public bool IsVisible
        {
            get { return _isVisible.Value; }
            set { this.RaiseAndSetIfChanged(_isVisible, value); }
        }

        #endregion

        public string ImageName { get; set; }

        public BindableCollection<IContextMenuItem> Items { get; private set; }

        public ContextMenuGroupItem(ISchedulerProvider scheduler, BindableCollection<IContextMenuItem> itemsCollection)
        {
            _isVisible.ConnectINPCProperty(this, () => IsVisible, scheduler).AddDisposable(Disposables);

            Items = itemsCollection;

            IsVisible = true;
        }
    }
}