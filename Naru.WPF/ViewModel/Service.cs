using System.Reactive.Disposables;

namespace Naru.WPF.ViewModel
{
    public abstract class Service : IService
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        protected CompositeDisposable Disposables
        {
            get { return _disposables; }
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}