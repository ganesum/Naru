using Naru.RX;
using Naru.WPF.Scheduler;
using Naru.WPF.ViewModel;

namespace Naru.WPF.MVVM
{
    public class HeaderViewModel : ViewModel.ViewModel
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

        public HeaderViewModel(IDispatcherSchedulerProvider scheduler)
        {
            _displayName.ConnectINPCProperty(this, () => DisplayName, scheduler).AddDisposable(Disposables);
            _imageName.ConnectINPCProperty(this, () => ImageName, scheduler).AddDisposable(Disposables);
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}