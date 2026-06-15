using System;
using System.Collections.Generic;
using System.Reflection;
using Studio.Core;
using Studio.Core.Services;
using Studio.Managers;
using UnityEngine;

namespace Studio.Debug
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class CheatAttribute : Attribute
    {
        public string Name { get; }
        public CheatAttribute(string name) => Name = name;
    }

    public sealed class DeveloperCheats : MonoBehaviour
    {
        private readonly Dictionary<string, Action> _cheats = new();

        private void Awake()
        {
#if !DEVELOPMENT_BUILD && !UNITY_EDITOR
            enabled = false;
            return;
#endif
            var gameManager = ServiceLocator.TryGet<GameManager>(out var gm) ? gm : null;
            if (gameManager != null && !gameManager.Context.Config.EnableDebugTools)
            {
                enabled = false;
                return;
            }

            RegisterCheats();
        }

        private void RegisterCheats()
        {
            var methods = GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            for (var i = 0; i < methods.Length; i++)
            {
                var attr = methods[i].GetCustomAttribute<CheatAttribute>();
                if (attr != null)
                {
                    _cheats[attr.Name] = (Action)Delegate.CreateDelegate(typeof(Action), this, methods[i]);
                }
            }
        }

        public IReadOnlyDictionary<string, Action> GetCheats() => _cheats;

        [Cheat("Add Currency")]
        private void AddCurrency()
        {
            if (ServiceLocator.TryGet<CurrencyManager>(out var currency))
            {
                currency.Earn(100);
            }
        }

        [Cheat("Go To Menu")]
        private void GoToMenu()
        {
            if (ServiceLocator.TryGet<GameManager>(out var gm))
            {
                gm.StateMachine.ChangeState(Studio.Gameplay.States.MenuState.StateId);
            }
        }
    }
}
