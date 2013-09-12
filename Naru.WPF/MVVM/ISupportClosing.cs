using System;

namespace Naru.WPF.MVVM
{
    public interface ISupportClosing
    {
        bool CanClose();

        event EventHandler CanCloseChanged;

        void Close();

        event EventHandler Closed;
    }
}