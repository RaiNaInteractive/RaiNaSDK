using System;

public sealed class CircularDependencyException : Exception
    {
        public CircularDependencyException(Type serviceType)
            : base(
                $"Circular dependency detected while resolving '{serviceType.FullName}'.")
        {
        }
    }
