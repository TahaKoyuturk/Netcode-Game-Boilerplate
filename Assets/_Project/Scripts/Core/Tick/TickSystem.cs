using System.Collections.Generic;
using Studio.Core.Services;
using UnityEngine;

namespace Studio.Core.Tick
{
    public sealed class TickSystem : MonoBehaviour, IService
    {
        private readonly List<ITickable> _tickables = new();
        private readonly List<IFixedTickable> _fixedTickables = new();
        private bool _isDirty;

        public void Register(ITickable tickable)
        {
            if (!_tickables.Contains(tickable))
            {
                _tickables.Add(tickable);
                _isDirty = true;
            }
        }

        public void Unregister(ITickable tickable)
        {
            _tickables.Remove(tickable);
        }

        public void RegisterFixed(IFixedTickable tickable)
        {
            if (!_fixedTickables.Contains(tickable))
            {
                _fixedTickables.Add(tickable);
                _isDirty = true;
            }
        }

        public void UnregisterFixed(IFixedTickable tickable)
        {
            _fixedTickables.Remove(tickable);
        }

        private void Update()
        {
            if (_isDirty)
            {
                _tickables.Sort((a, b) => a.TickPriority.CompareTo(b.TickPriority));
                _fixedTickables.Sort((a, b) => a.FixedTickPriority.CompareTo(b.FixedTickPriority));
                _isDirty = false;
            }

            var delta = Time.deltaTime;
            for (var i = 0; i < _tickables.Count; i++)
            {
                _tickables[i].OnTick(delta);
            }
        }

        private void FixedUpdate()
        {
            var delta = Time.fixedDeltaTime;
            for (var i = 0; i < _fixedTickables.Count; i++)
            {
                _fixedTickables[i].OnFixedTick(delta);
            }
        }
    }
}
