using System;
using System.Collections.Generic;

namespace RaiNa.Events
{
    public sealed class EventBus : IEventBus
    {
        private readonly Dictionary<Type, object> _channels = new();
        private readonly object _lock = new();

        public IEventContext<TEvent> Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var channel = GetOrCreateChannel<TEvent>();
            return channel.Publish(@event);
        }

        public void Subscribe<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
            => GetOrCreateChannel<TEvent>().Subscribe(handler);

        public void Unsubscribe<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
            => GetOrCreateChannel<TEvent>().Unsubscribe(handler);

        public void Clear<TEvent>() where TEvent : IEvent
        {
            lock (_lock)
            {
                _channels.Remove(typeof(TEvent));
            }
        }

        public void ClearAll()
        {
            lock (_lock)
            {
                _channels.Clear();
            }
        }

        private EventChannel<TEvent> GetOrCreateChannel<TEvent>() where TEvent : IEvent
        {
            var key = typeof(TEvent);
            lock (_lock)
            {
                if (!_channels.TryGetValue(key, out var raw))
                {
                    raw = new EventChannel<TEvent>();
                    _channels[key] = raw;
                }
                return (EventChannel<TEvent>)raw;
            }
        }
    }
}
