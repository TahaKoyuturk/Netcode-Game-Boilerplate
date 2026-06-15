using Studio.Core.Services;
using Studio.Systems.Tween;
using Studio.Systems.UI;
using UnityEngine;

namespace Studio.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIPanelBase : MonoBehaviour, IUIPanel
    {
        [SerializeField] protected string panelId;
        [SerializeField] private float fadeDuration = 0.2f;

        private CanvasGroup _canvasGroup;
        private ITweenService _tweenService;

        public string PanelId => panelId;
        public bool IsVisible { get; private set; }

        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (ServiceLocator.TryGet<ITweenService>(out var tween))
            {
                _tweenService = tween;
            }
            else
            {
                _tweenService = new NullTweenService();
            }

            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            gameObject.SetActive(false);
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            IsVisible = true;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            _tweenService.TweenAlpha(_canvasGroup, 1f, fadeDuration);
            OnShow();
        }

        public virtual void Hide()
        {
            IsVisible = false;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _tweenService.TweenAlpha(_canvasGroup, 0f, fadeDuration, () => gameObject.SetActive(false));
            OnHide();
        }

        protected virtual void OnShow() { }
        protected virtual void OnHide() { }
    }
}
