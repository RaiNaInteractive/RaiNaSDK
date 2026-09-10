namespace RaiNa.Events.Observer
{
    public interface IReadOnlyObservable<TObserver>
    {
        ObserverToken Subscribe(IObserver<TObserver> observer);
        void Unsubscribe(ObserverToken token);
    }
}
