using Microsoft.Practices.Unity;

namespace Naru.WPF.MVVM
{
    public class BindableCollectionFactory
    {
        private readonly IUnityContainer _container;

        public BindableCollectionFactory(IUnityContainer container)
        {
            _container = container;
        }

        public BindableCollection<T> Get<T>()
        {
            return _container.Resolve<BindableCollection<T>>();
        }
    }
}