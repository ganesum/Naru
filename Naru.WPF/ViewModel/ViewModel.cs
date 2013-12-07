using System.Reactive.Disposables;

namespace Naru.WPF.ViewModel
{
    public abstract class ViewModel : NotifyPropertyChanged, IViewModel
    {
        protected readonly CompositeDisposable Disposables = new CompositeDisposable();
    }
}