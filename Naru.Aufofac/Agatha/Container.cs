using System;
using Agatha.Common.InversionOfControl;
using Autofac;
using IContainer = Autofac.IContainer;

namespace Naru.Aufofac.Agatha
{
    public class Container : global::Agatha.Common.InversionOfControl.IContainer
    {
        private readonly ContainerBuilder _builder;
        private readonly Func<IContainer> _container;
        
        public Container(ContainerBuilder builder, Func<IContainer> container)
        {
            _builder = builder;
            _container = container;
        }

        public void Register(Type componentType, Type implementationType, Lifestyle lifeStyle)
        {
            _builder.RegisterType(implementationType).As(componentType).WithLifestyle(lifeStyle);
        }

        public void Register<TComponent, TImplementation>(Lifestyle lifestyle)
            where TImplementation : TComponent
        {
            _builder.RegisterType<TImplementation>().As<TComponent>().WithLifestyle(lifestyle);
        }

        public void RegisterInstance(Type componentType, object instance)
        {
            _builder.Register(x => instance).As(componentType);
        }

        public void RegisterInstance<TComponent>(TComponent instance)
        {
            _builder.Register(x => instance).AsSelf();
        }

        public TComponent Resolve<TComponent>()
        {
            return _container().Resolve<TComponent>();
        }

        public TComponent Resolve<TComponent>(string key)
        {
            return _container().ResolveKeyed<TComponent>(key);
        }

        public object Resolve(Type componentType)
        {
            return _container().Resolve(componentType);
        }

        public TComponent TryResolve<TComponent>()
        {
            TComponent instance;
            return _container().TryResolve(out instance) ? instance : default(TComponent);
        }

        public void Release(object component)
        {
        }
    }
}