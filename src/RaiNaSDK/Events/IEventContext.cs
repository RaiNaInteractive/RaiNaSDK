namespace RaiNa.Events
{
    public interface IEventContext<TEvent> where TEvent : IEvent
    {
        TEvent Event { get; }
    }
}
