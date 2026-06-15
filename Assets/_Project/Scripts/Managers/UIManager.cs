using System.Collections.Generic;
using Studio.Core.Services;
using Studio.Systems.UI;
using UnityEngine;

namespace Studio.Managers
{
    public sealed class UIManager : IManager
    {
        private readonly Dictionary<string, IUIPanel> _panels = new();
        private readonly Stack<IUIPanel> _panelStack = new();
        private Transform _uiRoot;

        public void Initialize()
        {
            var root = GameObject.Find("UIRoot");
            _uiRoot = root != null ? root.transform : null;
        }

        public void Shutdown()
        {
            _panels.Clear();
            _panelStack.Clear();
        }

        public void RegisterPanel(IUIPanel panel)
        {
            if (panel == null)
            {
                return;
            }

            _panels[panel.PanelId] = panel;
        }

        public void ShowPanel(string panelId)
        {
            if (!_panels.TryGetValue(panelId, out var panel))
            {
                Debug.LogWarning($"Panel '{panelId}' is not registered.");
                return;
            }

            panel.Show();
            _panelStack.Push(panel);
        }

        public void HideTopPanel()
        {
            if (_panelStack.Count == 0)
            {
                return;
            }

            var panel = _panelStack.Pop();
            panel.Hide();
        }

        public Transform GetUIRoot() => _uiRoot;
    }
}
