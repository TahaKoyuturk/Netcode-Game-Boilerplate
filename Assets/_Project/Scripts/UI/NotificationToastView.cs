using Studio.Core.Events;
using Studio.Systems.Notification;
using TMPro;
using UnityEngine;

namespace Studio.UI
{
    public sealed class NotificationToastView : EventBusBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI messageText;

        private void Start()
        {
            Subscribe<NotificationPublishedEvent>(OnNotification);
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
            }
        }

        private void OnNotification(NotificationPublishedEvent evt)
        {
            if (titleText != null) titleText.text = evt.Data.Title;
            if (messageText != null) messageText.text = evt.Data.Message;
            if (canvasGroup != null) canvasGroup.alpha = 1f;
        }
    }
}
