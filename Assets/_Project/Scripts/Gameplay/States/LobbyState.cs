using Studio.Core.Services;
using Studio.Core.StateMachine;
using Studio.Managers;
using Studio.Networking;

namespace Studio.Gameplay.States
{
    public sealed class LobbyState : IState
    {
        public const string StateId = "Lobby";
        string IState.StateId => StateId;

        public void OnEnter()
        {
            if (ServiceLocator.TryGet<UIManager>(out var ui))
            {
                ui.ShowPanel("Lobby");
            }
        }

        public void OnExit()
        {
            if (ServiceLocator.TryGet<LobbyManager>(out var lobby))
            {
                _ = lobby.LeaveLobbyAsync();
            }
        }

        public void OnTick(float deltaTime)
        {
            if (ServiceLocator.TryGet<LobbyManager>(out var lobby))
            {
                _ = lobby.SendHeartbeatAsync();
            }
        }
    }
}
