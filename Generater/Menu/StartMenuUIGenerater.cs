using System;
using Unity.AI.Navigation;
using UnityEngine;
using static MazeGame.MazeGameConstants.MazeConstants;
using static MazeGame.MazeGameConstants;

namespace MazeGame
{
    public class StartMenuUIGenerater : BaseGenerator
    {
        [SerializeField] MainMenuExplainWindowController MainMenuExplainWindowController;
        [SerializeField] MainMenuConfigWindowController MainMenuConfigWindowController;
        [SerializeField] MainMenuLevelWindowController MainMenuLevelWindowController;
        [SerializeField] MainMenulWindowController MainMenulWindowController;
        LevelSelection LevelSelection;
        public StartMenuUIGenerater(LevelSelection _levelSelection)
        {
            LevelSelection = _levelSelection;
        }

        public override void Init()
        {
            base.Init();
            MainMenulWindowController.WindowInit();
            MainMenuExplainWindowController.WindowInit();
            MainMenuConfigWindowController.WindowInit();
            MainMenuLevelWindowController.WindowInit(LevelSelection);
            MessageScrollManager.Instance.ManagerInit();
        }

        public override void Generated()
        {
            // “Á‚É‚È‚µ
            MainMenulWindowController.Begin();
            MessageScrollManager.Instance.ManagerStart();
        }

        public override void Tick()
        {
            // “Á‚É‚È‚µ

        }
        public override void Destroy()
        {
            // “Á‚É‚È‚µ
            MessageScrollManager.Instance.ManagerDestroy();
        }
    }
}
