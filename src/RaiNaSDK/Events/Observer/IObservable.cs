namespace RaiNa.Events.Observer
{
    public interface IObservable<TObserver>
    {
        void Notify(TObserver value);
        void Clear();
    }
}
