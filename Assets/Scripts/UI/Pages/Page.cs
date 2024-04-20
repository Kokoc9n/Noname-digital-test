using UnityEngine;
using DG.Tweening;

namespace Core.UI
{
    public abstract class Page : MonoBehaviour
    {
        private Vector3 startingScale;
        private void Start()
        {
            startingScale = transform.localScale;
        }
        public virtual void OnAppear()
        {
            gameObject.SetActive(true);
            transform.DOScale(startingScale, 0.3f).SetEase(Ease.OutSine);
        }
        public virtual void OnDisappear()
        {
            transform.DOScale(Vector3.one * 0.2f, 0.3f).SetEase(Ease.OutSine)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
}