using System;

namespace RaiNa.Services.Dependency
{
    public sealed class ServiceDescriptor
    {
        public Type ServiceType { get; }
        public ServiceLifetime Lifetime { get; }
        public Func<IServiceContainer, object> Factory { get; }

        public ServiceDescriptor(
            Type serviceType,
            ServiceLifetime lifetime,
            Func<IServiceContainer, object> factory)
        {
            ServiceType = serviceType;
            Lifetime = lifetime;
            Factory = factory;
        }
    }
}
