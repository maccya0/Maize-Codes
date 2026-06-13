using System;
using Unity.AI.Navigation;
using UnityEngine;
using static MazeGame.MazeGameConstants.MazeConstants;
using static MazeGame.MazeGameConstants;
using System.Collections.Generic;

namespace MazeGame
{
    public class StageGenerater : BaseGenerator
    {
        [SerializeField] StageCreate StageCreate;
        [SerializeField] CheckPointController[] CheckPoints;
        [SerializeField] GoalController GoalController;
        [SerializeField] StartController StartController;
        [SerializeField][Range(0, 255)] private uint enemyRange = 192;
        private LevelSelection LevelSelection;
        public StageGenerater(LevelSelection levelSelection)
        {
            LevelSelection = levelSelection;
        }

        public override void Init()
        {
            base.Init();
            List<GameObject> checkPoints = new List<GameObject>();
            for(int i=0;i< CheckPoints.Length;i++)
            {
                checkPoints.Add(CheckPoints[i].gameObject);
            }
            StageCreate.Init(StartController.gameObject,GoalController.gameObject, checkPoints.ToArray());
            EnemyManager.Instance.ManagerInit();
        }

        public override void Generated()
        {
            base.Generated();
            MazeData mazeData = LevelSelection.GetCurrentMazeData();
            Maze maze = Maze.Instance;

            maze.MakeMaze(
                mazeData.StageSize,
                mazeData.TrapNum,
                mazeData.EnemyNum,
                mazeData.ExtendRate,
                mazeData.CheckPointNum,
                mazeData.ItemNum
            );
            StageCreate.Begin();
            EnemyManager.Instance.ManagerStart();
            EnemyManager.Instance.GenerateEnemys(enemyRange, mazeData.EnemyNum, (int)LevelSelection.GetLevel());
        }

        public override void Tick()
        {
            base.Tick();
            // Œ»ó‚Í•s—v
        }

        public override void Destroy()
        {
            base.Destroy();
            EnemyManager.Instance.ManagerDestroy();
            StageCreate.Destroy();
        }
    }
}
