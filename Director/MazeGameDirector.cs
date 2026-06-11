using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static MazeGame.MazeGameConstants.MazeConstants;
using System.Collections.Generic;
using System.Collections;

namespace MazeGame
{
    public class GameDirector : MonoBehaviour
    {
        // ゲーム進捗関連
        [SerializeField] private ProgressManager progressManager;
        public bool IsAllCheckedPoints { get; private set; }
        [SerializeField] private JudgeManager judgeManager;
        [SerializeField] private float eventTime;
        [SerializeField] private MazeTimeEvent[] randamTimeEvents;
        [SerializeField] private MazeOnceEvent[] randamOnceEvents;
        [SerializeField] private StageCreate stageCreate;
        [SerializeField] private List<SoundData> gameBgm;
        private int onceEventTotal;
        private int timeEventTotal;
        private float erapsedTime;
        private GameManager gameManager;

        private void Awake()
        {
            if (!SceneManager.GetSceneByName("BackGroundScene").isLoaded)
            {
                SceneManager.LoadSceneAsync("BackGroundScene", LoadSceneMode.Additive);
            }
            SoundManager.Instance.CanPlaySound = false;

            CheckoutException();
            IsAllCheckedPoints = false;
            progressManager.CheckedAllPoints += CompleateAllChecked;
            judgeManager.SetState += SetGameState;

            gameManager = GameManager.Instance;
            erapsedTime = 0;
            InitMaze();
            timeEventTotal = 0;
            foreach (var timeEvent in randamTimeEvents)
            {
                timeEventTotal += timeEvent.rateVal;
            }
            onceEventTotal = 0;
            foreach (var onceEvent in randamOnceEvents)
            {
                onceEventTotal += onceEvent.rateVal;
            }
        }

        private IEnumerator Start()
        {
            yield return new WaitForSecondsRealtime(1.0f);
            SoundManager.Instance.CanPlaySound = true;
            int randamBGM = UnityEngine.Random.Range(0, gameBgm.Count);
            SoundManager.Instance.StartBgm(gameBgm[randamBGM]);
        }

        private void OnDestroy()
        {
            // ゲーム進捗関連のコールバック停止
            progressManager.CheckedAllPoints -= CompleateAllChecked;
            judgeManager.SetState -= SetGameState;
        }

        public void InitializeGame()
        {
            InitMaze();
        }

        private void SetGameState(GameState state)
        {
            gameManager.SetGameState(state);
        }

        private void CompleateAllChecked()
        {
            IsAllCheckedPoints = true;
        }


        private void CheckoutException()
        {
            if (progressManager == null)
            {
                throw new InvalidOperationException("ProgressManager未登録");
            }
            if (judgeManager == null)
            {
                throw new InvalidOperationException("JudgeManager");
            }
        }



        private void Update()
        {
            gameManager.CheckGameState();
            GenerateEvent();
        }

        private void GenerateEvent()
        {
            try
            {
                erapsedTime += Time.deltaTime;
                if (erapsedTime <= eventTime) return;
                erapsedTime = 0;
                bool target = UnityEngine.Random.Range(0, 2) == 1 ? true : false;
                MazeEvent mazeEvent = null;
                if (target)
                {
                    int randamRate = UnityEngine.Random.Range(0, timeEventTotal);
                    int weight = 0;
                    foreach (var timeEvent in randamTimeEvents)
                    {
                        weight += timeEvent.rateVal;
                        if (randamRate < weight)
                        {
                            mazeEvent = timeEvent;
                            break;
                        }
                    }
                    Debug.Log("SelectOnceEvent");
                }
                else
                {
                    int randamRate = UnityEngine.Random.Range(0, onceEventTotal);
                    int weight = 0;
                    foreach (var onceEvent in randamOnceEvents)
                    {
                        weight += onceEvent.rateVal;
                        if (randamRate < weight)
                        {
                            mazeEvent = onceEvent;
                            break;
                        }
                    }
                    Debug.Log("SelectTimeEvent");
                }
                if (mazeEvent != null)
                {
                    mazeEvent.TriggerEvent();
                }

            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private void InitMaze()
        {
            Maze maze = Maze.Instance;
            MazeData mazeData = LevelManager.Instance.GetCurrentMazeData();
            maze.MakeMaze(mazeData.StageSize, mazeData.TrapNum, mazeData.EnemyNum,mazeData.ExtendRate, mazeData.CheckPointNum, mazeData.ItemNum);
            stageCreate.Initialize();
        }
    }
}
