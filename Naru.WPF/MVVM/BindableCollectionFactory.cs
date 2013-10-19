using Microsoft.Practices.Unity;

using Naru.WPF.ViewModel;

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

        public ReactiveSingleSelectCollection<T> GetSingleSelect<T>()
        {
            return _container.Resolve<ReactiveSingleSelectCollection<T>>();
        }

        public ReactiveMultiSelectCollection<T> GetMultiSelect<T>()
        {
            return _container.Resolve<ReactiveMultiSelectCollection<T>>();
        }
    }
}