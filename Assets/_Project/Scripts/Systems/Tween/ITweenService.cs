using System;
using Studio.Core.Services;
using UnityEngine;

namespace Studio.Systems.Tween
{
    public interface ITweenService : IService
    {
        ITweenHandle TweenFloat(float from, float to, float duration, Action<float> onUpdate, Action onComplete = null);
        ITweenHandle TweenAlpha(CanvasGroup target, float to, float duration, Action onComplete = null);
        ITweenHandle Sequence(Action<ITweenSequence> build);
        void Kill(ITweenHandle handle);
        void KillAll();
    }

    public interface ITweenHandle
    {
        bool IsActive { get; }
        void Kill();
    }

    public interface ITweenSequence
    {
        ITweenSequence AppendFloat(float from, float to, float duration, Action<float> onUpdate);
        ITweenSequence AppendAlpha(CanvasGroup target, float to, float duration);
        ITweenSequence AppendCallback(Action callback);
        ITweenHandle Build();
    }
}
