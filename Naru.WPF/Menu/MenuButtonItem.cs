using Naru.WPF.Command;
using Naru.WPF.ViewModel;

namespace Naru.WPF.Menu
{
    public class MenuButtonItem : NotifyPropertyChanged, IMenuItem
    {
        public string DisplayName { get; set; }

        #region IsVisible

        private bool _isVisible;

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (value.Equals(_isVisible)) return;
                _isVisible = value;
                RaisePropertyChanged(() => IsVisible);
            }
        }

        #endregion

        public DelegateCommand Command { get; set; }

        public string ImageName { get; set; }

        public MenuButtonItem()
        {
            IsVisible = true;
        }
    }
}