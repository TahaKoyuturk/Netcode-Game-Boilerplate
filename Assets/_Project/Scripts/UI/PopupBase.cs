using System;
using Studio.Core.Services;
using Studio.Managers;
using Studio.Systems.Popup;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Studio.UI
{
    public class PopupBase : UIWindowBase, IPopup
    {
        [SerializeField] protected string popupId;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private TextMeshProUGUI confirmLabel;
        [SerializeField] private TextMeshProUGUI cancelLabel;

        private Action<PopupResult> _callback;

        public string PopupId => popupId;

        protected virtual void Start()
        {
            if (ServiceLocator.TryGet<PopupManager>(out var popupManager))
            {
                popupManager.RegisterPopup(this);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            if (confirmButton != null)
            {
                confirmButton.onClick.AddListener(() => Complete(PopupResult.Confirmed));
            }

            if (cancelButton != null)
            {
                cancelButton.onClick.AddListener(() => Complete(PopupResult.Cancelled));
            }
        }

        public void Open(PopupRequest request, Action<PopupResult> callback)
        {
            _callback = callback;
            if (titleText != null) titleText.text = request.Title;
            if (messageText != null) messageText.text = request.Message;
            if (confirmLabel != null) confirmLabel.text = request.ConfirmText;
            if (cancelLabel != null) cancelLabel.text = request.CancelText;
            if (cancelButton != null) cancelButton.gameObject.SetActive(request.ShowCancel);
            Show();
        }

        protected void Complete(PopupResult result)
        {
            Hide();
            _callback?.Invoke(result);
            _callback = null;
        }

        public override void Close()
        {
            Complete(PopupResult.Cancelled);
        }
    }
}
