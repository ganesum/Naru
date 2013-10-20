using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using Naru.WPF.MVVM;

namespace Naru.WPF.ViewModel
{
    public class ReactiveMultiSelectCollection<T> : NotifyPropertyChanged, IReactiveMultiSelect<T>
    {
        private readonly Subject<IEnumerable<T>> _selectedItemChanged = new Subject<IEnumerable<T>>(); 

        public BindableCollection<T> Items { get; private set; }

        #region SelectedItem

        private IEnumerable<T> _selectedItems;

        public IEnumerable<T> SelectedItems
        {
            get { return _selectedItems; }
            set
            {
                _selectedItems = value;
                RaisePropertyChanged(() => SelectedItems);

                _selectedItemChanged.OnNext(value);
            }
        }

        #endregion

        public IObservable<IEnumerable<T>> SelectedItemsChanged { get { return _selectedItemChanged.AsObservable(); } }

        public ReactiveMultiSelectCollection(BindableCollectionFactory bindableCollectionFactory)
        {
            Items = bindableCollectionFactory.Get<T>();
        }
    }
}