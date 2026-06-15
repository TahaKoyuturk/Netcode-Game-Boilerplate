using Studio.Core.Services;
using Studio.Core.StateMachine;
using Studio.Managers;
using UnityEngine;

namespace Studio.Gameplay.States
{
    public sealed class PauseState : IState
    {
        public const string StateId = "Pause";
        string IState.StateId => StateId;

        public void OnEnter()
        {
            Time.timeScale = 0f;
            if (ServiceLocator.TryGet<UIManager>(out var ui))
            {
                ui.ShowPanel("Pause");
            }
        }

        public void OnExit()
        {
            Time.timeScale = 1f;
        }

        public void OnTick(float deltaTime)
        {
        }
    }
}
