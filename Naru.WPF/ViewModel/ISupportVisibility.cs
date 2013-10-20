using System;

namespace Naru.WPF.ViewModel
{
    public interface ISupportVisibility
    {
        bool IsVisible { get; }

        void Show();

        void Hide();

        IObservable<bool> IsVisibleChanged { get; }
    }
}