using System;

using Naru.WPF.ContextMenu;
using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;

namespace Naru.WPF.ViewModel
{
    public class ReactiveSingleSelectCollection<T> : ViewModel, IReactiveSingleSelect<T>
    {
        public BindableCollection<T> Items { get; private set; }

        #region SelectedItem

        private readonly ObservableProperty<T> _selectedItem = new ObservableProperty<T>();

        public T SelectedItem
        {
            get { return _selectedItem.Value; }
            set { _selectedItem.Value = value; }
        }

        public IObservable<T> SelectedItemChanged { get { return _selectedItem.ValueChanged; } }

        #endregion

        public ReactiveSingleSelectCollection(BindableCollection<T> itemsCollection, ISchedulerProvider scheduler)
        {
            Items = itemsCollection;

            _selectedItem.ConnectINPCProperty(this, () => SelectedItem, scheduler).AddDisposable(Disposables);
        }
    }
}