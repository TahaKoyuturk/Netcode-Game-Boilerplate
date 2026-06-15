using Studio.Core;
using Studio.Core.Services;
using Studio.Core.StateMachine;
using Studio.Managers;

namespace Studio.Gameplay.States
{
    public sealed class GameplayState : IState
    {
        public const string StateId = "Gameplay";
        string IState.StateId => StateId;

        public void OnEnter()
        {
            var gameManager = ServiceLocator.Get<GameManager>();
            var sceneName = gameManager.Context.Config.GameplayScene;
            if (ServiceLocator.TryGet<LoadingManager>(out var loading))
            {
                _ = loading.LoadSceneAsync(sceneName);
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
