using System;

namespace Naru.WPF.MVVM
{
    public interface ISupportClosing
    {
        void Close();

        event EventHandler OnClosed;
    }
}