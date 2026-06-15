using Studio.Core;
using Studio.Core.Services;
using Studio.Managers;
using UnityEngine;

namespace Studio.Debug
{
    public sealed class CommandConsole : MonoBehaviour
    {
        private string _input = "";
        private bool _visible;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                _visible = !_visible;
            }
        }

        private void OnGUI()
        {
#if !DEVELOPMENT_BUILD && !UNITY_EDITOR
            return;
#endif
            if (!_visible)
            {
                return;
            }

            GUILayout.BeginArea(new Rect(10, Screen.height - 80, Screen.width - 20, 70), GUI.skin.window);
            GUILayout.BeginHorizontal();
            _input = GUILayout.TextField(_input);
            if (GUILayout.Button("Run", GUILayout.Width(60)))
            {
                Execute(_input);
                _input = "";
            }

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void Execute(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                return;
            }

            var parts = command.Split(' ');
            switch (parts[0].ToLower())
            {
                case "help":
                    UnityEngine.Debug.Log("Commands: help, state <id>, currency <amount>, scene <name>");
                    break;
                case "state":
                    if (parts.Length > 1 && ServiceLocator.TryGet<GameManager>(out var gm))
                    {
                        gm.StateMachine.ChangeState(parts[1]);
                    }
                    break;
                case "currency":
                    if (parts.Length > 1 && int.TryParse(parts[1], out var amount) &&
                        ServiceLocator.TryGet<CurrencyManager>(out var currency))
                    {
                        currency.SetBalance(amount);
                    }
                    break;
                case "scene":
                    if (parts.Length > 1 && ServiceLocator.TryGet<LoadingManager>(out var loading))
                    {
                        _ = loading.LoadSceneAsync(parts[1]);
                    }
                    break;
            }
        }
    }
}
