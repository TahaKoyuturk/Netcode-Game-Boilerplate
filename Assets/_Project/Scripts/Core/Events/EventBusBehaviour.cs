using System;
using System.Collections.Generic;
using UnityEngine;

namespace Studio.Core.Events
{
    public abstract class EventBusBehaviour : MonoBehaviour
    {
        private readonly List<Action> _unsubscribeActions = new();

        protected void Subscribe<T>(Action<T> handler) where T : IEvent
        {
            EventBus.Subscribe(handler);
            _unsubscribeActions.Add(() => EventBus.Unsubscribe(handler));
        }

        protected virtual void OnDestroy()
        {
            for (var i = 0; i < _unsubscribeActions.Count; i++)
            {
                _unsubscribeActions[i].Invoke();
            }

            _unsubscribeActions.Clear();
        }
    }
}
