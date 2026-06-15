using Studio.Core.Services;
using Studio.Core.StateMachine;
using Studio.Managers;
using Studio.Networking;

namespace Studio.Gameplay.States
{
    public sealed class GameOverState : IState
    {
        public const string StateId = "GameOver";
        string IState.StateId => StateId;

        public void OnEnter()
        {
            if (ServiceLocator.TryGet<UIManager>(out var ui))
            {
                ui.ShowPanel("GameOver");
            }

            if (ServiceLocator.TryGet<NetworkManagerWrapper>(out var network))
            {
                network.Shutdown();
            }
        }

        public void OnExit()
        {
        }

        public void OnTick(float deltaTime)
        {
        }
    }
}
