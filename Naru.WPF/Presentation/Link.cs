using System.Windows.Input;

using Naru.RX;
using Naru.WPF.Scheduler;
using Naru.WPF.ViewModel;

namespace Naru.WPF.Presentation
{
    /// <summary>
    /// Represents a displayable link.
    /// </summary>
    public class Link : ViewModel.ViewModel
    {
        #region DisplayName

        private readonly ObservableProperty<string> _displayName = new ObservableProperty<string>();

        public string DisplayName
        {
            get { return _displayName.Value; }
            set { _displayName.RaiseAndSetIfChanged(value); }
        }

        #endregion

        public ICommand Command { get; set; }

        public Link(IDispatcherSchedulerProvider scheduler)
        {
            _displayName.ConnectINPCProperty(this, () => DisplayName, scheduler).AddDisposable(Disposables);
        }
    }
}