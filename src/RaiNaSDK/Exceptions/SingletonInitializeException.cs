using System;

namespace RaiNa.Exceptions
{
    /// <summary>
    /// Exception thrown when singleton initialization fails.
    /// </summary>
    public class SingletonInitializeException : Exception
    {
        public SingletonInitializeException(Type instanceType) : 
            base($"Invalid operation to create a new instance of {instanceType.Name}.\n" +
                 $"Use {instanceType.Name}.Instance instead.")
        {
        }
    }
}
