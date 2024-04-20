using UnityEngine;

namespace Core.UI
{
    public class ConnectionLostButton : ButtonClickHandler
    {
        public override void OnButtonClicked()
        {
            CanvasManager.PopPage();
        }
        public override void Start()
        {
            base.Start();
        }
    }
}