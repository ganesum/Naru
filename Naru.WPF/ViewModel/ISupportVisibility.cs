using System;

using Naru.Core;

namespace Naru.WPF.ViewModel
{
    public interface ISupportVisibility
    {
        bool IsVisible { get; }

        void Show();

        void Hide();

        event EventHandler<DataEventArgs<bool>> IsVisibleChanged;
    }
}