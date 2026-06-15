namespace Studio.Systems.UI
{
    public interface IUIPanel
    {
        string PanelId { get; }
        void Show();
        void Hide();
    }
}
