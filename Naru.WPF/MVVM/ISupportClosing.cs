using System;

namespace Naru.WPF.MVVM
{
    public interface ISupportClosing
    {
        bool CanClose();

        void Close();

        event EventHandler Closed;
    }
}