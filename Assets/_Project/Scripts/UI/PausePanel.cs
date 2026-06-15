using Studio.Core.Services;
using Studio.Core.StateMachine;
using UnityEngine;
using UnityEngine.UI;

namespace Studio.UI
{
    public sealed class PausePanel : UIWindowBase
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button menuButton;

        protected override void Awake()
        {
            base.Awake();
            panelId = "Pause";
            if (resumeButton != null) resumeButton.onClick.AddListener(OnResume);
            if (menuButton != null) menuButton.onClick.AddListener(OnMenu);
        }

        private void Start()
        {
            if (ServiceLocator.TryGet<Managers.UIManager>(out var ui))
            {
                ui.RegisterPanel(this);
            }
        }

        private void OnResume()
        {
            if (ServiceLocator.TryGet<Core.GameManager>(out var gm))
            {
                gm.StateMachine.PopState();
            }
        }

        private void OnMenu()
        {
            if (ServiceLocator.TryGet<Core.GameManager>(out var gm))
            {
                gm.StateMachine.ChangeState(GameStateIds.Menu);
            }
        }
    }
}
