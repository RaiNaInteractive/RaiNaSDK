using System;

namespace RaiNa.Events
{
    public static class GlobalEventBus
    {
        private static IEventBus _instance = new EventBus();

        public static IEventBus Bus => GetOrCreateBus();
 
        public static void SetBus(IEventBus bus)
            => _instance = bus ?? throw new ArgumentNullException(nameof(bus));
 
        public static IEventContext<TEvent> Publish<TEvent>(TEvent @event)
            where TEvent : IEvent
            => _instance.Publish(@event);
 
        public static void Subscribe<TEvent>(IEventHandler<TEvent> handler)
            where TEvent : IEvent
            => _instance.Subscribe(handler);
 
        public static void Unsubscribe<TEvent>(IEventHandler<TEvent> handler)
            where TEvent : IEvent
            => _instance.Unsubscribe(handler);

        private static IEventBus GetOrCreateBus()
        {
            _instance ??= new EventBus();

            return _instance;
        }
    }
}
