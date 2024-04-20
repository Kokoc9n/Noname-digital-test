using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    [RequireComponent(typeof(Button))]
    public abstract class ButtonClickHandler : MonoBehaviour
    {
        private Button button;
        private Tween tween;
        public virtual void Start()
        {
            tween = transform.DOShakeScale(0.2f, 0.2f).Pause().SetAutoKill(false);
            button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClicked);
            button.onClick.AddListener(() => tween.Restart());
        }
        public abstract void OnButtonClicked();
    }
}