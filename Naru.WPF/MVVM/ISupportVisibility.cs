using System;

using Naru.Core;

namespace Naru.WPF.MVVM
{
    public interface ISupportVisibility
    {
        bool IsVisible { get; }

        void Show();

        void Hide();

        event EventHandler<DataEventArgs<bool>> IsVisibleChanged;
    }
}