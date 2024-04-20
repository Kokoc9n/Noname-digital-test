using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class TabIndicationManager : MonoBehaviour
    {
        public static TabIndicationManager Instance;
        [SerializeField] Color highlightColor;
        [SerializeField] Color defaultColor;
        private List<TabIndication> list = new();
        private struct TabIndication
        {
            public TMP_Text text;
            public Image image;
        }
        private void Awake()
        {
            if (Instance == null) Instance = this;
            foreach (Transform child in transform)
            {
                TabIndication tabIndication = new();
                tabIndication.image = child.GetChild(0).GetComponent<Image>();
                tabIndication.text = child.GetChild(1).GetComponent<TMP_Text>();

                list.Add(tabIndication);
            }
        }
        public void UpdateActiveTabHighlight(int activeTabIndex)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (i == activeTabIndex)
                    list[i].text.color = list[i].image.color = highlightColor;
                else list[i].text.color = list[i].image.color = defaultColor;
            }
        }
    }
}