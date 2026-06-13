using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MazeGame
{
    public class MainMenuUIWindowManager : BaseManager<MainMenuUIWindowManager>
    {
        [SerializeField] private GameObject configWindow;
        [SerializeField] private GameObject menuWindow;
        [SerializeField] private GameObject levelUI;
        [SerializeField] private GameObject explainUI;

        private GameObject[] allWindows;

        protected override void Awake()
        {
            base.Awake();
            if (Instance != this) return;

            allWindows = new GameObject[] { configWindow, menuWindow, levelUI, explainUI };
        }

        private void SwitchWindow(GameObject targetWindow)
        {
            foreach (var window in allWindows)
            {
                if (window != null)
                {
                    window.SetActive(window == targetWindow);
                }
            }
        }



        public void ActiveLevelUI() => SwitchWindow(levelUI);
        public void ActiveLevelExplain() => SwitchWindow(explainUI);
        public void ActiveConfigWindow() => SwitchWindow(configWindow);
        public void ActiveMenuWindow() => SwitchWindow(menuWindow);
    }
}