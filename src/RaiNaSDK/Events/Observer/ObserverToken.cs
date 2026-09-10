namespace RaiNa.Events.Observer
{
    public readonly struct ObserverToken
    {
        internal readonly int Id;

        internal ObserverToken(int id)
        {
            Id = id;
        }

        public bool IsValid => Id >= 0;
    }
}
