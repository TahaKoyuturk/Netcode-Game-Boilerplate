namespace Studio.UI
{
    public sealed class GameOverPanel : UIPanelBase
    {
        protected override void Awake()
        {
            base.Awake();
            panelId = "GameOver";
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
