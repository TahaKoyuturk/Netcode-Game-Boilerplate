namespace Studio.UI
{
    public sealed class LobbyPanel : UIPanelBase
    {
        protected override void Awake()
        {
            base.Awake();
            panelId = "Lobby";
        }

        private void Start()
        {
            if (Studio.Core.Services.ServiceLocator.TryGet<Managers.UIManager>(out var ui))
            {
                ui.RegisterPanel(this);
            }
        }
    }
}
