using System;
using System.Collections.Generic;

namespace Naru.WPF.ViewModel
{
    public interface IReactiveMultiSelect<T>
    {
        IEnumerable<T> SelectedItems { get; set; }

        IObservable<IEnumerable<T>> SelectedItemsChanged { get; } 
    }
}