using Studio.Core.Services;
using Studio.Core.StateMachine;
using Studio.Managers;

namespace Studio.Gameplay.States
{
    public sealed class MenuState : IState
    {
        public const string StateId = "Menu";
        string IState.StateId => StateId;

        public void OnEnter()
        {
            if (ServiceLocator.TryGet<UIManager>(out var ui))
            {
                ui.ShowPanel("MainMenu");
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
