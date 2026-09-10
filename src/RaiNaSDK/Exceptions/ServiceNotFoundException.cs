using System;


namespace RaiNa.Exceptions
{
    public sealed class ServiceNotFoundException : Exception
    {
        public ServiceNotFoundException(Type serviceType)
            : base($"Service '{serviceType.FullName}' is not registered.")
        {
        }
    }
}