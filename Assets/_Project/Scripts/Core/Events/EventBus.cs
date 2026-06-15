using System;
using System.Collections.Generic;

namespace Studio.Core.Events
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, Delegate> Handlers = new();

        public static void Subscribe<T>(Action<T> handler) where T : IEvent
        {
            var type = typeof(T);
            if (Handlers.TryGetValue(type, out var existing))
            {
                Handlers[type] = Delegate.Combine(existing, handler);
            }
            else
            {
                Handlers[type] = handler;
            }
        }

        public static void Unsubscribe<T>(Action<T> handler) where T : IEvent
        {
            var type = typeof(T);
            if (!Handlers.TryGetValue(type, out var existing))
            {
                return;
            }

            var updated = Delegate.Remove(existing, handler);
            if (updated == null)
            {
                Handlers.Remove(type);
            }
            else
            {
                Handlers[type] = updated;
            }
        }

        public static void Publish<T>(T eventData) where T : IEvent
        {
            if (!Handlers.TryGetValue(typeof(T), out var handler))
            {
                return;
            }

            ((Action<T>)handler).Invoke(eventData);
        }

        public static void Clear()
        {
            Handlers.Clear();
        }
    }
}
