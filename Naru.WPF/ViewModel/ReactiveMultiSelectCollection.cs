using System;
using System.Collections.Generic;

using Naru.RX;
using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;

namespace Naru.WPF.ViewModel
{
    public class ReactiveMultiSelectCollection<T> : ViewModel, IReactiveMultiSelect<T>
    {
        public BindableCollection<T> Items { get; private set; }

        #region SelectedItems

        private readonly ObservableProperty<IEnumerable<T>> _selectedItems = new ObservableProperty<IEnumerable<T>>();

        public IEnumerable<T> SelectedItems
        {
            get { return _selectedItems.Value; }
            set {  _selectedItems.Value = value; }
        }

        #endregion

        public IObservable<IEnumerable<T>> SelectedItemsChanged { get { return _selectedItems.ValueChanged; } }

        public ReactiveMultiSelectCollection(BindableCollection<T> itemsCollection, ISchedulerProvider scheduler)
        {
            Items = itemsCollection;

            _selectedItems.ConnectINPCProperty(this, () => SelectedItems, scheduler).AddDisposable(Disposables);
        }
    }
}