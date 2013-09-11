using Microsoft.Practices.Prism.ViewModel;

namespace Naru.WPF.MVVM
{
    public class HeaderViewModel : NotificationObject, IViewModel
    {
        #region DisplayName

        private string _displayName;

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                if (value == _displayName) return;
                _displayName = value;
                RaisePropertyChanged(() => DisplayName);
            }
        }

        #endregion

        #region ImageName

        private string _imageName;

        public string ImageName
        {
            get { return _imageName; }
            set
            {
                if (value == _imageName) return;
                _imageName = value;
                RaisePropertyChanged(() => ImageName);
            }
        }

        #endregion

        public override string ToString()
        {
            return DisplayName;
        }
    }
}