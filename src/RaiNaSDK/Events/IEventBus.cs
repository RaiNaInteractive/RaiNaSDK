namespace RaiNa.Events
{
    public interface IEventBus
    {
        IEventContext<TEvent> Publish<TEvent>(TEvent @event)
            where TEvent : IEvent;
        void Subscribe<TEvent>(IEventHandler<TEvent> handler)
            where TEvent : IEvent;
        void Unsubscribe<TEvent>(IEventHandler<TEvent> handler)
            where TEvent : IEvent;
        void Clear<TEvent>() where TEvent : IEvent;
        void ClearAll();
    }
}
