using System;
using Unity.AI.Navigation;
using UnityEngine;
using static MazeGame.MazeGameConstants.MazeConstants;
using static MazeGame.MazeGameConstants;
using System.Collections.Generic;

namespace MazeGame
{
    public class GameSystemGenerater : BaseGenerator
    {
        private GameJudger gameJudger;
        private CheckPointProgress CheckPointProgress;

        public GameJudger GetGameJudger() { return gameJudger; }
        [SerializeField] GoalController GoalController;
        [SerializeField] PlayerController PlayerController;
        [SerializeField] int MaxDeathNum = 3;
        [SerializeField] CheckPointController[] CheckPoints;
        [SerializeField] private List<SoundData> gameBgm;
        [SerializeField] private MazeEventController eventController;

        public GameSystemGenerater()
        {
        }

        public override void Init()
        {
            base.Init();
            SoundManager.Instance.CanPlaySound = false;
            TimeManager.Instance.ManagerInit();
            gameJudger = new GameJudger(GoalController,PlayerController, MaxDeathNum);
            CheckPointProgress = new CheckPointProgress(CheckPoints);
            CheckPointProgress.CheckedAllPoints += ListenCheckPoints;
            eventController.Init();
        }

        public override void Generated()
        {
            base.Generated();
            TimeManager.Instance.ManagerStart();
            gameJudger.Start();
            CheckPointProgress.Start();
            eventController.StartEvents();
            SoundManager.Instance.CanPlaySound = true;
            SoundData randamBGM = gameBgm[UnityEngine.Random.Range(0, gameBgm.Count)];
            TimeManager.Instance.StartGame();
        }

        public override void Tick()
        {
            base.Tick();
            TimeManager.Instance.Tick();
            eventController?.Tick();

        }

        public override void Destroy()
        {
            base.Destroy();
            eventController.StopEvents();
            TimeManager.Instance.ManagerDestroy();
            gameJudger?.Dispose();
            CheckPointProgress?.Dispose();
        }

        public override void EndGame()
        {
            base.EndGame();
            TimeManager.Instance.StopGame();
        }

        private void ListenCheckPoints()
        {
            if (CheckPointProgress != null)
            {
                CheckPointProgress.CheckedAllPoints -= ListenCheckPoints;
            }
            gameJudger.CheckdAllPoints();
        }
    }
}
