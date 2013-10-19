using System;

namespace Naru.WPF.ViewModel
{
    public interface IReactiveSingleSelect<T>
    {
        T SelectedItem { get; set; }

        IObservable<T> SelectedItemChanged { get; } 
    }
}