namespace RaiNa.Events
{
    public interface IEventChannel<TEvent> where TEvent : IEvent
    {
        IEventContext<TEvent> Publish(TEvent @event);
        void Subscribe(IEventHandler<TEvent> handler);
        void Unsubscribe(IEventHandler<TEvent> handler);
    }
}
