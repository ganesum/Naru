using Naru.WPF.Command;
using Naru.WPF.ViewModel;

namespace Naru.WPF.Presentation
{
    /// <summary>
    /// Represents a displayable link.
    /// </summary>
    public class Link : NotifyPropertyChanged
    {
        #region DisplayName

        private string _displayName;

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                if (_displayName != value)
                {
                    _displayName = value;
                    RaisePropertyChanged(() => DisplayName);
                }
            }
        }

        #endregion

        public DelegateCommand Command { get; set; }
    }
}