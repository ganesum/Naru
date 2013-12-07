using System.ComponentModel;

namespace Naru.WPF.ViewModel
{
    public interface INotifyPropertyChangedEx : INotifyPropertyChanged
    {
        void ConnectINPC(string propertyName);
    }
}