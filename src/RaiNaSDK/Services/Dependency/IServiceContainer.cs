namespace RaiNa.Services.Dependency
{
    public interface IServiceContainer : IServiceProvider, IServiceRegistry
    {
        public string Name { get; }
        public bool IsRoot { get; }
        public IServiceContainer Parent { get; }

        IServiceContainer CreateChildContainer(string containerName);
        IServiceContainer GetChildContainer(string containerName, bool recursive = false);
        void AttachChildContainer(IServiceContainer container);
        void RemoveChildContainer(string containerName, bool recursivc = false);
        void RemoveChildContainer(IServiceContainer container);
        IServiceContainer GetParentContainer(string containerName, bool recursive = false);
        public void ClearAll();
    }
}
