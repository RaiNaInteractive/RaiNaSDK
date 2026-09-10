using System;
using System.Collections.Generic;

namespace RaiNa.Events.Observer
{
    public sealed class Observable<T> : IReadOnlyObservable<T>
    {
        private readonly List<Subscription<T>> _subscriptions = new();

        private int _nextId;

        public ObserverToken Subscribe(IObserver<T> observer)
        {
            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            Subscription<T> subscription = new()
            {
                Id = _nextId++,
                Observer = observer
            };

            _subscriptions.Add(subscription);

            return new ObserverToken(subscription.Id);
        }

        public void Unsubscribe(ObserverToken token)
        {
            for (int i = 0; i < _subscriptions.Count; i++)
            {
                if (_subscriptions[i].Id != token.Id)
                {
                    continue;
                }

                _subscriptions.RemoveAt(i);
                return;
            }
        }

        public void Notify(T value)
        {
            for (int i = 0; i < _subscriptions.Count; i++)
            {
                _subscriptions[i].Observer.OnNotify(value);
            }
        }

        public void Clear()
        {
            _subscriptions.Clear();
        }
    }
}
