namespace Core.UI
{
    public class BottomTabBar : Page
    {
        public override void OnAppear()
        {
            gameObject.SetActive(true);
            CanvasManager.StackPage(typeof(AppsPage));
            // Cringe alert.
            TabIndicationManager.Instance.UpdateActiveTabHighlight(0);
        }

        public override void OnDisappear()
        {
            base.OnDisappear();
        }
    }
}