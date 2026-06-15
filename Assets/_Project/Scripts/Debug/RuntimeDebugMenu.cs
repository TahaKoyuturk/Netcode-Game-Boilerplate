using Studio.Core.Services;
using Studio.Managers;
using UnityEngine;

namespace Studio.Debug
{
    public sealed class RuntimeDebugMenu : MonoBehaviour
    {
        [SerializeField] private bool showOnStart;

        private bool _visible;
        private DeveloperCheats _cheats;
        private Vector2 _scroll;

        private void Start()
        {
#if !DEVELOPMENT_BUILD && !UNITY_EDITOR
            enabled = false;
            return;
#endif
            _cheats = FindAnyObjectByType<DeveloperCheats>();
            _visible = showOnStart;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                _visible = !_visible;
            }
        }

        private void OnGUI()
        {
            if (!_visible)
            {
                return;
            }

            GUILayout.BeginArea(new Rect(10, 10, 300, 400), GUI.skin.window);
            GUILayout.Label($"FPS: {(1f / Mathf.Max(Time.deltaTime, 0.0001f)):F0}");

            if (ServiceLocator.TryGet<CurrencyManager>(out var currency))
            {
                GUILayout.Label($"Currency: {currency.Balance}");
            }

            _scroll = GUILayout.BeginScrollView(_scroll);
            if (_cheats != null)
            {
                foreach (var cheat in _cheats.GetCheats())
                {
                    if (GUILayout.Button(cheat.Key))
                    {
                        cheat.Value.Invoke();
                    }
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
    }
}
