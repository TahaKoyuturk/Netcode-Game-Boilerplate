using Studio.Core.Events;
using Studio.Systems.Popup;

namespace Studio.Systems.Popup
{
    public readonly struct PopupOpenedEvent : IEvent
    {
        public readonly PopupRequest Request;

        public PopupOpenedEvent(PopupRequest request)
        {
            Request = request;
        }
    }

    public readonly struct PopupClosedEvent : IEvent
    {
        public readonly PopupResult Result;

        public PopupClosedEvent(PopupResult result)
        {
            Result = result;
        }
    }
}
