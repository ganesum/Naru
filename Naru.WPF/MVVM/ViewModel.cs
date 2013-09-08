using Common.Logging;

using Microsoft.Practices.Prism.ViewModel;

namespace Naru.WPF.MVVM
{
    public abstract class ViewModel : NotificationObject, IViewModel
    {
        protected readonly ILog Log;

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

        protected ViewModel(ILog log)
        {
            Log = log;

            DisplayName = string.Empty;
        }
    }
}