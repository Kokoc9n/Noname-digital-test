using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    [RequireComponent(typeof(Button))]
    public class PageChanger : ButtonClickHandler
    {
        [SerializeField] PageType pageType;
        public override void OnButtonClicked()
        {
            Type type = null;
            switch (pageType)
            {
                case PageType.AppsPage:
                    type = typeof(AppsPage);
                    break;
                case PageType.GamesPage:
                    type = typeof(GamesPage);
                    break;
                case PageType.ModsPage:
                    type = typeof(ModsPage);
                    ModsDisplayManager.Instance.OnModsTabEnter();
                    break;
                case PageType.TopicsPage:
                    type = typeof(TopicsPage);
                    break;
                default:
                    break;
            }
            TabIndicationManager.Instance.UpdateActiveTabHighlight((int)pageType);
            CanvasManager.SwitchPage(type);
        }
        
    }
    enum PageType
    {
        AppsPage = 0,
        GamesPage = 1,
        ModsPage = 2,
        TopicsPage = 3
    }
}