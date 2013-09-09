using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;

using Naru.WPF.TPL;

namespace Naru.WPF.MVVM
{
    public class BindableCollection<T> : ObservableCollection<T>
    {
        private readonly IScheduler _scheduler;

        public BindableCollection(IScheduler scheduler)
        {
            _scheduler = scheduler;
            IsNotifying = true;
        }

        public bool IsNotifying { get; set; }

        public virtual void NotifyOfPropertyChange(string propertyName)
        {
            if (IsNotifying)
                _scheduler.Dispatcher.ExecuteSync(() => OnPropertyChanged(new PropertyChangedEventArgs(propertyName)));
        }

        public void Refresh()
        {
            _scheduler.Dispatcher.ExecuteSync(RefreshInternal);
        }

        public Task RefreshAsync()
        {
            return Task.Factory.StartNew(RefreshInternal, _scheduler.Dispatcher);
        }

        private void RefreshInternal()
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected override sealed void InsertItem(int index, T item)
        {
            _scheduler.Dispatcher.ExecuteSync(() => InsertItemBase(index, item));
        }

        protected virtual void InsertItemBase(int index, T item)
        {
            base.InsertItem(index, item);
        }

        protected override sealed void MoveItem(int oldIndex, int newIndex)
        {
            _scheduler.Dispatcher.ExecuteSync(() => MoveItemBase(oldIndex, newIndex));
        }

        protected virtual void MoveItemBase(int oldIndex, int newIndex)
        {
            base.MoveItem(oldIndex, newIndex);
        }

        protected override sealed void SetItem(int index, T item)
        {
            _scheduler.Dispatcher.ExecuteSync(() => SetItemBase(index, item));
        }

        protected virtual void SetItemBase(int index, T item)
        {
            base.SetItem(index, item);
        }

        protected override sealed void RemoveItem(int index)
        {
            _scheduler.Dispatcher.ExecuteSync(() => RemoveItemBase(index));
        }

        protected virtual void RemoveItemBase(int index)
        {
            base.RemoveItem(index);
        }

        public Task ClearAsync()
        {
            return Task.Factory.StartNew(Clear, _scheduler.Dispatcher);
        }

        protected override sealed void ClearItems()
        {
            _scheduler.Dispatcher.ExecuteSync(ClearItemsBase);
        }

        protected virtual void ClearItemsBase()
        {
            base.ClearItems();
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (IsNotifying)
            {
                base.OnCollectionChanged(e);
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (IsNotifying)
            {
                base.OnPropertyChanged(e);
            }
        }

        public void AddRange(IEnumerable<T> items)
        {
            _scheduler.Dispatcher.ExecuteSync(() =>
            {
                AddRangeInternal(items);

                RefreshInternal();
            });
        }

        public Task AddRangeAsync(IEnumerable<T> items)
        {
            return Task.Factory
                .StartNew(() => AddRangeInternal(items), _scheduler.Dispatcher)
                .SelectMany(RefreshAsync, _scheduler.Dispatcher);
        }

        private void AddRangeInternal(IEnumerable<T> items)
        {
            var previousNotificationSetting = IsNotifying;
            IsNotifying = false;
            var index = Count;
            foreach (var item in items)
            {
                InsertItemBase(index, item);
                index++;
            }
            IsNotifying = previousNotificationSetting;
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            _scheduler.Dispatcher.ExecuteSync(() =>
            {
                RemoveRangeInternal(items);

                RefreshInternal();
            });
        }

        public Task RemoveRangeAsync(IEnumerable<T> items)
        {
            return Task.Factory
                .StartNew(() => RemoveRangeInternal(items), _scheduler.Dispatcher)
                .SelectMany(RefreshAsync, _scheduler.Dispatcher);
        }

        private void RemoveRangeInternal(IEnumerable<T> items)
        {
            var previousNotificationSetting = IsNotifying;
            IsNotifying = false;
            foreach (var item in items)
            {
                var index = IndexOf(item);
                if (index >= 0)
                {
                    RemoveItemBase(index);
                }
            }
            IsNotifying = previousNotificationSetting;
        }
    }
}