using System;
using Studio.Core.Save;
using Studio.Core.Services;
using Studio.Data.Save;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Studio.Systems.Input
{
    public sealed class InputBindingService : IService, IDisposable
    {
        public const string SaveKey = "input_bindings";

        private readonly ISaveService _saveService;
        private readonly GameInputActions _actions;

        public InputBindingService(ISaveService saveService, InputActionAsset actionsAsset)
        {
            _saveService = saveService;
            _actions = new GameInputActions(actionsAsset);
            LoadBindings();
        }

        public GameInputActions Actions => _actions;

        public void StartRebind(InputAction action, int bindingIndex, Action<bool> onComplete)
        {
            if (action == null)
            {
                onComplete?.Invoke(false);
                return;
            }

            action.Disable();
            action.PerformInteractiveRebinding(bindingIndex)
                .OnComplete(operation =>
                {
                    operation.Dispose();
                    action.Enable();
                    SaveBindings();
                    onComplete?.Invoke(true);
                })
                .OnCancel(operation =>
                {
                    operation.Dispose();
                    action.Enable();
                    onComplete?.Invoke(false);
                })
                .Start();
        }

        public void SaveBindings()
        {
            var data = new InputBindingsSaveData
            {
                BindingsJson = _actions.ExportBindings()
            };
            _saveService.Save(SaveKey, data);
        }

        public void LoadBindings()
        {
            if (_saveService.TryLoad(SaveKey, out InputBindingsSaveData data))
            {
                _actions.ImportBindings(data.BindingsJson);
            }
        }

        public void ResetBindings()
        {
            _actions.ResetBindings();
            _saveService.Delete(SaveKey);
        }

        public void Dispose()
        {
            _actions.Dispose();
        }
    }
}
