using Studio.Core.Networking;
using Studio.Core.Services;
using Studio.Core.StateMachine;
using UnityEngine;
using UnityEngine.UI;

namespace Studio.UI
{
    public sealed class MainMenuPanel : UIPanelBase
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button hostButton;
        [SerializeField] private Button quitButton;

        protected override void Awake()
        {
            base.Awake();
            panelId = "MainMenu";

            if (playButton != null) playButton.onClick.AddListener(OnPlay);
            if (hostButton != null) hostButton.onClick.AddListener(OnHost);
            if (quitButton != null) quitButton.onClick.AddListener(OnQuit);
        }

        private void Start()
        {
            if (ServiceLocator.TryGet<Managers.UIManager>(out var ui))
            {
                ui.RegisterPanel(this);
            }
        }

        private void OnPlay()
        {
            if (ServiceLocator.TryGet<Core.GameManager>(out var gm))
            {
                gm.StateMachine.ChangeState(GameStateIds.Gameplay);
            }
        }

        private async void OnHost()
        {
            if (!ServiceLocator.TryGet<INetworkHostService>(out var networkHost))
            {
                return;
            }

            await networkHost.HostGameAsync("StudioLobby");
            if (ServiceLocator.TryGet<Core.GameManager>(out var gm))
            {
                gm.StateMachine.ChangeState(GameStateIds.Lobby);
            }
        }

        private void OnQuit()
        {
            Application.Quit();
        }
    }
}
