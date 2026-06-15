using System;

namespace Studio.Systems.Popup
{
    public interface IPopup
    {
        string PopupId { get; }
        void Open(PopupRequest request, Action<PopupResult> callback);
    }
}
