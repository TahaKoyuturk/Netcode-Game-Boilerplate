using System;

namespace Studio.Systems.Popup
{
    public sealed class PopupRequest
    {
        public string PopupId;
        public string Title;
        public string Message;
        public string ConfirmText = "OK";
        public string CancelText = "Cancel";
        public bool ShowCancel = true;
        public Action<PopupResult> Callback;
    }

    public enum PopupResult
    {
        Confirmed,
        Cancelled
    }
}
