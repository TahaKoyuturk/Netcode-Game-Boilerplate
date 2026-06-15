namespace Studio.UI
{
    public sealed class AlertPopup : PopupBase
    {
        protected override void Awake()
        {
            base.Awake();
            popupId = "AlertPopup";
        }
    }
}
