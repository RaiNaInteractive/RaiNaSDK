namespace RaiNa.Events.Observer
{
    public interface IObserver<in TObserver>
    {
        void OnNotify(TObserver value);
    }
}
