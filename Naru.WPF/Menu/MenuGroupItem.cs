using Naru.WPF.ContextMenu;
using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.ViewModel;

namespace Naru.WPF.Menu
{
    public class MenuGroupItem : ViewModel.ViewModel, IMenuItem
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

        public BindableCollection<IMenuItem> Items { get; private set; }

        public MenuGroupItem(BindableCollection<IMenuItem> itemsCollection, ISchedulerProvider scheduler)
        {
            _isVisible.ConnectINPCProperty(this, () => IsVisible, scheduler).AddDisposable(Disposables);

            Items = itemsCollection;

            IsVisible = true;
        }
    }
}