namespace Studio.UI
{
    public sealed class ConfirmPopup : PopupBase
    {
        protected override void Awake()
        {
            base.Awake();
            popupId = "ConfirmPopup";
        }
    }
}
