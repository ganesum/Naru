using System.Reactive.Disposables;

namespace Naru.WPF.ViewModel
{
    public abstract class ViewModel : NotifyPropertyChanged, IViewModel
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        protected CompositeDisposable Disposables
        {
            get { return _disposables; }
        }
    }
}