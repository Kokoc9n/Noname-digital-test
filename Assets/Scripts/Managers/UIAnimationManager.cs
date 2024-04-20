using UnityEngine;
using DG.Tweening;

namespace Core.UI
{
    public static class UIAnimationManager
    {
        public static Tween SlideToPosition(GameObject gameObject, float targetY, float duration)
        {
            return gameObject.transform.DOMoveY(targetY, duration)
                .SetEase(Ease.OutSine);
        }
    }
}