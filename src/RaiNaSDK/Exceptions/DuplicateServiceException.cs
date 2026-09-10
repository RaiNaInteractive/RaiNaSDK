using System;

namespace RaiNa.Exceptions
{
    public sealed class DuplicateServiceException : Exception
    {
        public DuplicateServiceException(Type serviceType) :
            base($"Service '{serviceType.FullName}' already registered.")
        {
        }
    }
}