using System;
using Studio.Core.Services;
using Studio.Systems.Input;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Studio.UI
{
    public class RebindUIController : MonoBehaviour
    {
        [SerializeField] private InputActionReference actionReference;
        [SerializeField] private int bindingIndex;
        [SerializeField] private Button rebindButton;
        [SerializeField] private Button resetButton;
        [SerializeField] private TextMeshProUGUI bindingLabel;

        private InputBindingService _bindingService;

        private void Start()
        {
            if (ServiceLocator.TryGet<InputBindingService>(out _bindingService))
            {
                RefreshLabel();
            }

            if (rebindButton != null)
            {
                rebindButton.onClick.AddListener(StartRebind);
            }

            if (resetButton != null)
            {
                resetButton.onClick.AddListener(ResetBindings);
            }
        }

        private void StartRebind()
        {
            if (_bindingService == null || actionReference == null)
            {
                return;
            }

            _bindingService.StartRebind(actionReference.action, bindingIndex, success =>
            {
                if (success)
                {
                    RefreshLabel();
                }
            });
        }

        private void ResetBindings()
        {
            _bindingService?.ResetBindings();
            RefreshLabel();
        }

        private void RefreshLabel()
        {
            if (bindingLabel == null || actionReference == null)
            {
                return;
            }

            bindingLabel.text = actionReference.action.GetBindingDisplayString(bindingIndex);
        }
    }
}
