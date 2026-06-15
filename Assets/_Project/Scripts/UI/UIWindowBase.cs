using Studio.Core.Services;
using Studio.Systems.Input;
using UnityEngine;
using UnityEngine.UI;

namespace Studio.UI
{
    public abstract class UIWindowBase : UIPanelBase
    {
        [SerializeField] private Button closeButton;

        protected override void Awake()
        {
            base.Awake();
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(Close);
            }
        }

        protected virtual void Update()
        {
            if (!IsVisible)
            {
                return;
            }

            if (ServiceLocator.TryGet<InputBindingService>(out var input) &&
                input.Actions.UI["Cancel"].WasPressedThisFrame())
            {
                Close();
            }
        }

        public virtual void Close()
        {
            Hide();
        }
    }
}
