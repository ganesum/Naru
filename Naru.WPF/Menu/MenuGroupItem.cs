using Naru.RX;
using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.ViewModel;

namespace Naru.WPF.Menu
{
    public class MenuGroupItem : ViewModel.ViewModel, IMenuItem
    {
        #region DisplayName

        private readonly ObservableProperty<string> _displayName = new ObservableProperty<string>();

        public string DisplayName
        {
            get { return _displayName.Value; }
            set { _displayName.RaiseAndSetIfChanged(value); }
        }

        #endregion

        #region ImageName

        private readonly ObservableProperty<string> _imageName = new ObservableProperty<string>();

        public string ImageName
        {
            get { return _imageName.Value; }
            set { _imageName.RaiseAndSetIfChanged(value); }
        }

        #endregion

        public BindableCollection<IMenuItem> Items { get; private set; }

        #region IsVisible

        private readonly ObservableProperty<bool> _isVisible = new ObservableProperty<bool>();

        public bool IsVisible
        {
            get { return _isVisible.Value; }
            set { _isVisible.RaiseAndSetIfChanged(value); }
        }

        #endregion

        public MenuGroupItem(BindableCollection<IMenuItem> itemsCollection, ISchedulerProvider scheduler)
        {
            _displayName.ConnectINPCProperty(this, () => DisplayName, scheduler).AddDisposable(Disposables);
            _imageName.ConnectINPCProperty(this, () => ImageName, scheduler).AddDisposable(Disposables);
            _isVisible.ConnectINPCProperty(this, () => IsVisible, scheduler).AddDisposable(Disposables);

            Items = itemsCollection;

            IsVisible = true;
        }
    }
}