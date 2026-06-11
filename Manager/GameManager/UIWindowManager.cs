using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MazeGame
{
    public class UIWindowManager : BaseManager<UIWindowManager>
    {
        [SerializeField] private GameObject configWindow;
        [SerializeField] private GameObject resultWindow;
        [SerializeField] private GameObject gameUI;
        [SerializeField] private GameObject levelUI;
        [SerializeField] private GameObject explainUI;

        [SerializeField] private GameObject returnGameButton;
        [SerializeField] private List<SliderController> sliderControllers;

        private GameObject[] allWindows;

        protected override void Awake()
        {
            base.Awake();
            if (Instance != this) return;

            allWindows = new GameObject[] { configWindow, resultWindow, gameUI, levelUI, explainUI };
        }

        public override void ManagerInit()
        {
            foreach (var controller in sliderControllers)
            {
                controller.SetupSlider();
            }
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

        public void ActiveConfigWindow()
        {
            SwitchWindow(configWindow);
            EventSystem.current.SetSelectedGameObject(returnGameButton);
        }

        public void ActiveResultWindow() => SwitchWindow(resultWindow);
        public void ActiveGameUI() => SwitchWindow(gameUI);
        public void ActiveLevelUI() => SwitchWindow(levelUI);
        public void ActiveLevelExplain() => SwitchWindow(explainUI);
    }
}