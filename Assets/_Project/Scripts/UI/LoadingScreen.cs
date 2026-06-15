using Studio.Core.Events;
using EventBus = Studio.Core.Events.EventBus;
using Studio.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Studio.UI
{
    public sealed class LoadingScreen : UIPanelBase
    {
        [SerializeField] private Slider progressBar;
        [SerializeField] private TextMeshProUGUI statusText;

        protected override void OnShow()
        {
            base.OnShow();
            EventBus.Subscribe<Studio.Systems.Loading.LoadingProgressEvent>(OnProgress);
            EventBus.Subscribe<Studio.Systems.Loading.LoadingCompletedEvent>(OnCompleted);
        }

        protected override void OnHide()
        {
            EventBus.Unsubscribe<Studio.Systems.Loading.LoadingProgressEvent>(OnProgress);
            EventBus.Unsubscribe<Studio.Systems.Loading.LoadingCompletedEvent>(OnCompleted);
            base.OnHide();
        }

        private void OnProgress(Studio.Systems.Loading.LoadingProgressEvent evt)
        {
            if (progressBar != null)
            {
                progressBar.value = evt.Progress;
            }

            if (statusText != null)
            {
                statusText.text = ZStringUtility.FormatLoadingStatus(evt.Progress);
            }
        }

        private void OnCompleted(Studio.Systems.Loading.LoadingCompletedEvent evt)
        {
            Hide();
        }
    }
}
