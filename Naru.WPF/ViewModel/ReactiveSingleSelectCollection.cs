using System;
using System.Reactive.Subjects;

using Naru.WPF.MVVM;

namespace Naru.WPF.ViewModel
{
    public class ReactiveSingleSelectCollection<T> : NotifyPropertyChanged, IReactiveSingleSelect<T>
    {
        private readonly Subject<T> _selectedItemChanged = new Subject<T>(); 

        public BindableCollection<T> Items { get; private set; }

        #region SelectedItem

        private T _selectedItem;

        public T SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value; 
                RaisePropertyChanged(() => SelectedItem);

                _selectedItemChanged.OnNext(value);
            }
        }

        public IObservable<T> SelectedItemChanged { get; private set; }

        #endregion

        public ReactiveSingleSelectCollection(BindableCollectionFactory bindableCollectionFactory)
        {
            Items = bindableCollectionFactory.Get<T>();
        }
    }
}