using UnityEngine;
using DG.Tweening;

namespace Core
{
    public class LoadingCurtain : MonoBehaviour
    {
        public static LoadingCurtain Instance;
        public CanvasGroup Curtain;
        private void Awake()
        {
            if (Instance == null) Instance = this;
            DontDestroyOnLoad(this);
        }
        public void Show()
        {
            gameObject.SetActive(true);
            Curtain.alpha = 1;
        }
        public void Hide() => DOVirtual.Float(1,
                                              0,
                                              2f,
                                              f => { Curtain.alpha = f; })
                                              .SetEase(Ease.Linear)
            .OnComplete(() => gameObject.SetActive(false));
    }
}