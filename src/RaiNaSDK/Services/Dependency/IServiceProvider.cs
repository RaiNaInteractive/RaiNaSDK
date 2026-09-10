namespace RaiNa.Services.Dependency
{
    public interface IServiceProvider
    {
        T Resolve<T>() where T : class;
        bool TryResolve<T>(out T service) where T : class;
    }
}
