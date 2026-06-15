using System;
using System.Collections.Generic;
using Studio.Core.Events;

namespace Studio.Systems.Notification
{
    public readonly struct NotificationPublishedEvent : IEvent
    {
        public readonly NotificationData Data;

        public NotificationPublishedEvent(NotificationData data)
        {
            Data = data;
        }
    }

    public sealed class NotificationChannel
    {
        private readonly Queue<NotificationData> _queue = new();
        private NotificationData _current;
        private float _timer;
        private bool _isShowing;

        public void Enqueue(NotificationData data)
        {
            _queue.Enqueue(data);
            if (!_isShowing)
            {
                ShowNext();
            }
        }

        public void Tick(float deltaTime)
        {
            if (!_isShowing || _current == null)
            {
                return;
            }

            _timer -= deltaTime;
            if (_timer <= 0f)
            {
                _isShowing = false;
                _current = null;
                ShowNext();
            }
        }

        private void ShowNext()
        {
            if (_queue.Count == 0)
            {
                return;
            }

            _current = _queue.Dequeue();
            _timer = Math.Max(0.5f, _current.Duration);
            _isShowing = true;
            EventBus.Publish(new NotificationPublishedEvent(_current));
        }
    }
}
