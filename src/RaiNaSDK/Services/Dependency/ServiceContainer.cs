using System;
using System.Collections.Generic;

namespace RaiNa.Services.Dependency
{
    public sealed class ServiceContainer : IServiceContainer, IDisposable
    {
        public string Name { get; private set; }
        public bool IsRoot => Parent == null;
        public IServiceContainer Parent { get; private set; }

        private readonly Dictionary<Type, ServiceDescriptor> _descriptors = new();
        private readonly Dictionary<Type, object> _localInstances = new();

        private readonly Dictionary<string, IServiceContainer> _children = new();
        private bool _disposed;

        public ServiceContainer(string containerName, IServiceContainer parent)
        {
            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException(nameof(containerName));

            Name = containerName;
            Parent = parent;
        }

        public ServiceContainer(string containerName) : this(containerName, null) { }

        public void Register(ServiceDescriptor descriptor)
        {
            var type = descriptor.ServiceType;
            if (_descriptors.ContainsKey(type))
                throw new InvalidOperationException($"Service {type.Name} is already registered in '{Name}'.");

            _descriptors[type] = new ServiceDescriptor(type, descriptor.Lifetime, descriptor.Factory);
        }

        public void Register<T>(ServiceLifetime lifetime, Func<IServiceContainer, object> factory) where T : class =>
            Register(new ServiceDescriptor(typeof(T), lifetime, factory));

        public void Unregister(Type type)
        {
            if (_localInstances.TryGetValue(type, out object service))
            {
                if (service is IDisposable disposable)
                    disposable.Dispose();
            }

            _localInstances.Remove(type);
            _descriptors.Remove(type);
        }

        public void Unregister<T>() where T : class => Unregister(typeof(T));

        public T Resolve<T>() where T : class
        {
            if (TryResolveInternal(typeof(T), out var service))
                return (T)service;

            throw new KeyNotFoundException($"Service {typeof(T).Name} not found in '{Name}' or its parents.");
        }

        public bool TryResolve<T>(out T service) where T : class
        {
            if (TryResolveInternal(typeof(T), out var result))
            {
                service = (T)result;
                return true;
            }

            service = null;
            return false;
        }

        private bool TryResolveInternal(Type type, out object service)
        {
            var descriptor = FindDescriptor(type);
            if (descriptor == null)
            {
                service = null;
                return false;
            }

            if (descriptor.Lifetime == ServiceLifetime.Singleton)
            {
                if (!IsRoot && Parent is ServiceContainer parentContainer)
                {
                    return parentContainer.TryResolveInternal(type, out service);
                }
                return GetOrCreateInstance(type, descriptor, out service);
            }

            if (descriptor.Lifetime == ServiceLifetime.Scoped)
            {
                return GetOrCreateInstance(type, descriptor, out service);
            }

            service = descriptor.Factory(this);
            return true;
        }

        private ServiceDescriptor FindDescriptor(Type type)
        {
            if (_descriptors.TryGetValue(type, out var descriptor)) return descriptor;
            if (Parent != null && Parent is ServiceContainer parentContainer) return parentContainer.FindDescriptor(type);
            return null;
        }

        private bool GetOrCreateInstance(Type type, ServiceDescriptor descriptor, out object service)
        {
            if (_localInstances.TryGetValue(type, out service)) return true;

            service = descriptor.Factory(this);
            _localInstances[type] = service;
            return true;
        }

        public IServiceContainer CreateChildContainer(string childName)
        {
            var child = new ServiceContainer(childName, this);
            AttachChildContainer(child);
            return child;
        }

        public void AttachChildContainer(IServiceContainer child)
        {
            if (child.Parent != null && child.Parent != this)
                throw new InvalidOperationException($"Container '{child.Name}' already has a parent!");

            if (_children.ContainsKey(child.Name))
                throw new InvalidOperationException($"A child with ID '{child.Name}' already exists in '{Name}'.");

            _children.Add(child.Name, child);

            if (child is ServiceContainer concreteChild)
                concreteChild.Parent = this;
        }

        public IServiceContainer GetChildContainer(string searchName, bool recursive = false)
        {
            if (_children.TryGetValue(searchName, out var foundChild)) return foundChild;

            if (recursive)
            {
                foreach (var child in _children.Values)
                {
                    var deepFound = child.GetChildContainer(searchName, true);
                    if (deepFound != null) return deepFound;
                }
            }
            return null;
        }

        public IServiceContainer GetParentContainer(string searchName, bool recursive = false)
        {
            if (Parent == null) return null;
            if (Parent.Name == searchName) return Parent;

            if (recursive) return Parent.GetParentContainer(searchName, true);

            return null;
        }

        public void RemoveChildContainer(string containerName, bool recursive = false)
        {
            IServiceContainer container = GetChildContainer(containerName, recursive);

            if (container == null) return;

            container.ClearAll();
        }

        public void RemoveChildContainer(IServiceContainer container) =>
            container.ClearAll();

        public void ClearAll() => Dispose();

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                foreach(var child in _children.Values)
                {
                    child.ClearAll();

                    var temp = new List<Type>(_localInstances.Keys);
                    temp.ForEach(m => Unregister(m));
                }

                _descriptors.Clear();
                _localInstances.Clear();
                _children.Clear();
            }

            _disposed = true;
        }

        ~ServiceContainer()
        {
             Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
