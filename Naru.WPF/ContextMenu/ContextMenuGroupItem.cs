using Naru.WPF.MVVM;
using Naru.WPF.ViewModel;

namespace Naru.WPF.ContextMenu
{
    public class ContextMenuGroupItem : NotifyPropertyChanged, IContextMenuItem
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

        public string ImageName { get; set; }

        public BindableCollection<IContextMenuItem> Items { get; private set; }

        public ContextMenuGroupItem(BindableCollectionFactory bindableCollectionFactory)
        {
            Items = bindableCollectionFactory.Get<IContextMenuItem>();

            IsVisible = true;
        }
    }
}