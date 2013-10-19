using System;

namespace Naru.WPF.ViewModel
{
    public interface ISupportClosing
    {
        bool CanClose();

        void Close();

        event EventHandler Closed;
    }
}