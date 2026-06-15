using System.Collections.Generic;
using Studio.Core.Events;
using Studio.Core.Services;
using EventBus = Studio.Core.Events.EventBus;
using Studio.Systems.Popup;
using UnityEngine;

namespace Studio.Managers
{
    public sealed class PopupManager : IManager
    {
        private readonly Dictionary<string, IPopup> _popups = new();
        private readonly Queue<PopupRequest> _queue = new();
        private IPopup _activePopup;

        public void Initialize()
        {
        }

        public void Shutdown()
        {
            _popups.Clear();
            _queue.Clear();
            _activePopup = null;
        }

        public void RegisterPopup(IPopup popup)
        {
            if (popup == null)
            {
                return;
            }

            _popups[popup.PopupId] = popup;
        }

        public void Show(PopupRequest request)
        {
            _queue.Enqueue(request);
            if (_activePopup == null)
            {
                ShowNext();
            }
        }

        private void ShowNext()
        {
            if (_queue.Count == 0)
            {
                return;
            }

            var request = _queue.Dequeue();
            if (!_popups.TryGetValue(request.PopupId, out var popup))
            {
                Debug.LogWarning($"Popup '{request.PopupId}' not found.");
                request.Callback?.Invoke(PopupResult.Cancelled);
                ShowNext();
                return;
            }

            _activePopup = popup;
            EventBus.Publish(new PopupOpenedEvent(request));
            popup.Open(request, result =>
            {
                _activePopup = null;
                EventBus.Publish(new PopupClosedEvent(result));
                request.Callback?.Invoke(result);
                ShowNext();
            });
        }
    }
}
