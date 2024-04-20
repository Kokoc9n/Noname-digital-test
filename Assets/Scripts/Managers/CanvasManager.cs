using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Core.UI
{
    public class CanvasManager : MonoBehaviour
    {
        private static protected Page[] pagesToLoad;
        private static protected Stack<Page> activePages = new();

        public static CanvasManager Instance;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            pagesToLoad = FindObjectsOfType<Page>();
        }
        private void Start()
        {
            foreach (Page p in pagesToLoad) p.gameObject.SetActive(false);
            // Starting Page
            //StackPage(typeof(BottomTabBar));
        }
        public static void StackPage(Type pageType)
        {
            Page page = pagesToLoad.FirstOrDefault(p => p.GetType() == pageType);
            activePages.Push(page);

            page.OnAppear();
        }
        public static void PopPage()
        {
            activePages.Peek().OnDisappear();
            activePages.Pop();
        }
        public static void SwitchPage(Type pageType)
        {
            if (activePages.First().GetType() == pageType) return;
            activePages.First().OnDisappear();
            activePages.Pop();

            Page page = pagesToLoad.FirstOrDefault(p => p.GetType() == pageType);
            page.OnAppear();
            activePages.Push(page);
        }
    }
}