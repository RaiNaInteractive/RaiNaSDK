namespace RaiNa.Events
{
    public sealed class EventContext<TEvent> : IEventContext<TEvent> where TEvent : IEvent
    {
        public TEvent Event { get; }
 
        public EventContext(TEvent @event)
        {
            Event = @event;
        }
    }
}
