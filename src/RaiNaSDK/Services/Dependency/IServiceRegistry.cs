using System;

namespace RaiNa.Services.Dependency
{
    public interface IServiceRegistry
    {
        void Register(ServiceDescriptor descriptor);
        void Register<T>(ServiceLifetime lifetime, Func<IServiceContainer, object> factory) where T : class;

        void Unregister(Type type);
        void Unregister<T>() where T : class;
    }
}
