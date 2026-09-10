namespace RaiNaSDK;

public interface ISingleton
{
    void OnInitialized();
    void OnShutdown();
}

public interface ISingletonTracker
{
    void OnTracking(ISingleton singleton);
    void OnShutdown(ISingleton singleton);
}

public abstract class Singleton<T> where T : class, ISingleton
{
    private static T? _instance;
    private static object? _lock;
    private static ISingletonTracker[]? _activeTrackers;

    public static T Instance => Volatile.Read(ref _instance)
        ?? throw new InvalidOperationException($"Singleton '{typeof(T).Name}' is not initialized.");

    public static bool IsInitialized => Volatile.Read(ref _instance) != null;

    public static T Initialize(Func<T> factory, params ISingletonTracker[] trackers)
    {
        ArgumentNullException.ThrowIfNull(factory);

        if (Volatile.Read(ref _instance) != null)
            return _instance!;

        return LazyInitializer.EnsureInitialized(ref _instance, ref _lock, () =>
        {
            T instance = factory();

            instance.OnInitialized();

            if (trackers is { Length: > 0 })
            {
                _activeTrackers = trackers;
                for (int i = 0; i < trackers.Length; i++)
                {
                    trackers[i]?.OnTracking(instance);
                }
            }

            return instance;
        });
    }

    public static void Shutdown()
    {
        lock (_lock ??= new object())
        {
            T? instance = Volatile.Read(ref _instance);
            if (instance == null) return;

            instance.OnShutdown();

            if (_activeTrackers != null)
            {
                for (int i = 0; i < _activeTrackers.Length; i++)
                {
                    _activeTrackers[i]?.OnShutdown(instance);
                }
                _activeTrackers = null;
            }

            Volatile.Write(ref _instance, null);
        }
    }
}
