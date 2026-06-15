using System;
using System.Collections.Generic;
using UnityEngine;

namespace Studio.Systems.Tween
{
    public sealed class NullTweenService : ITweenService
    {
        public ITweenHandle TweenFloat(float from, float to, float duration, Action<float> onUpdate, Action onComplete = null)
        {
            onUpdate?.Invoke(to);
            onComplete?.Invoke();
            return new NullTweenHandle();
        }

        public ITweenHandle TweenAlpha(CanvasGroup target, float to, float duration, Action onComplete = null)
        {
            if (target != null)
            {
                target.alpha = to;
            }

            onComplete?.Invoke();
            return new NullTweenHandle();
        }

        public ITweenHandle Sequence(Action<ITweenSequence> build)
        {
            var sequence = new NullTweenSequence();
            build?.Invoke(sequence);
            sequence.Execute();
            return new NullTweenHandle();
        }

        public void Kill(ITweenHandle handle)
        {
            handle?.Kill();
        }

        public void KillAll()
        {
        }

        private sealed class NullTweenHandle : ITweenHandle
        {
            public bool IsActive => false;
            public void Kill() { }
        }

        private sealed class NullTweenSequence : ITweenSequence
        {
            private readonly List<Action> _steps = new();

            public ITweenSequence AppendFloat(float from, float to, float duration, Action<float> onUpdate)
            {
                _steps.Add(() => onUpdate?.Invoke(to));
                return this;
            }

            public ITweenSequence AppendAlpha(CanvasGroup target, float to, float duration)
            {
                _steps.Add(() =>
                {
                    if (target != null)
                    {
                        target.alpha = to;
                    }
                });
                return this;
            }

            public ITweenSequence AppendCallback(Action callback)
            {
                _steps.Add(() => callback?.Invoke());
                return this;
            }

            public ITweenHandle Build()
            {
                Execute();
                return new NullTweenHandle();
            }

            public void Execute()
            {
                for (var i = 0; i < _steps.Count; i++)
                {
                    _steps[i].Invoke();
                }
            }
        }
    }
}
