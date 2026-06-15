using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Studio.Systems.Input
{
    public sealed class GameInputActions : IDisposable
    {
        private readonly InputActionAsset _asset;

        public GameInputActions(InputActionAsset actionsAsset)
        {
            if (actionsAsset == null)
            {
                throw new ArgumentNullException(nameof(actionsAsset));
            }

            _asset = UnityEngine.Object.Instantiate(actionsAsset);
            _asset.Enable();
        }

        public InputActionAsset Asset => _asset;

        public InputActionMap Player => _asset.FindActionMap("Player", true);
        public InputActionMap UI => _asset.FindActionMap("UI", true);

        public void Dispose()
        {
            _asset.Disable();
            UnityEngine.Object.Destroy(_asset);
        }

        public string ExportBindings()
        {
            return _asset.SaveBindingOverridesAsJson();
        }

        public void ImportBindings(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                _asset.LoadBindingOverridesFromJson(json);
            }
        }

        public void ResetBindings()
        {
            _asset.RemoveAllBindingOverrides();
        }
    }
}
