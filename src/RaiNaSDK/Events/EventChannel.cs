using System.Collections.Generic;

namespace RaiNa.Events
{
    public sealed class EventChannel<TEvent> : IEventChannel<TEvent> where TEvent : IEvent
    {
        private readonly List<IEventHandler<TEvent>> _handlers = new();
        private bool _handlersDirty;

        public IEventContext<TEvent> Publish(TEvent @event)
        {
            if (_handlersDirty)
            {
                _handlers.Sort((a, b) => a.Priority.CompareTo(b.Priority));
                _handlersDirty = false;
            }

            var context = new EventContext<TEvent>(@event);
            return context;
        }

        public void Subscribe(IEventHandler<TEvent> handler)
        {
            if (!_handlers.Contains(handler))
            {
                _handlers.Add(handler);
                _handlersDirty = true;
            }
        }

        public void Unsubscribe(IEventHandler<TEvent> handler)
            => _handlers.Remove(handler);
    }
}
