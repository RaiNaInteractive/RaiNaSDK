namespace RaiNa.Events.Observer
{
    public sealed class Subscription<TObserver>
    {
        public int Id;

        public IObserver<TObserver> Observer;
    }
}
