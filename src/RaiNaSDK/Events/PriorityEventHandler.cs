using System;

namespace RaiNa.Events
{
    public sealed class PriorityEventHandler<TEvent> : IEventHandler<TEvent> where TEvent : IEvent
    {
        private readonly Action<IEventContext<TEvent>> _action;

        public int Priority { get; } = 0;
 
        public PriorityEventHandler(Action<IEventContext<TEvent>> action, int priority)
        {
            _action   = action;
            Priority  = priority;
        }
 
        public void Handle(IEventContext<TEvent> context) => _action(context);
    }
}
