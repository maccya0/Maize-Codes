using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static MazeGame.MazeGameConstants.MazeConstants;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace MazeGame
{

    public enum GameState
    {
        StartMenu,
        GamePlay,
        End
    }

    public class SystemDirector : BaseDirector<SystemDirector>
    {
        [SerializeField] private MazeData[] mazeDatas;
        [SerializeField] Scene StartScene;
        [SerializeField] Scene MazeScene;
        private LevelSelection LevelSelection;
        public LevelSelection GetLevelSelection() { return LevelSelection; }
        // ステートパターン管理用
        private Dictionary<GameState, GameStateBase> StateMap;
        private GameStateBase CurrentState;
        private bool IsTransitioning;

        private void Awake()
        {
            DirectorInit();
        }

        private async void Start()
        {
            await DirectorStart();
        }

        private void Update()
        {
            if (!IsTransitioning && CurrentState != null)
            {
                CurrentState.OnRuntimeState();
            }
            DirectorRunTime();
        }

        private void OnDestroy()
        {
            // No Care
        }

        protected override void DirectorInit()
        {
            LevelSelection = new LevelSelection(mazeDatas);
            StateMap = new Dictionary<GameState, GameStateBase>
            {
                { GameState.StartMenu   , new StartMenuState(this) },
                { GameState.GamePlay    , new GamePlayState(this) }, 
                { GameState.End         , new EndState(this) }
            };
        }

        protected override async Task DirectorStart()
        {
            await InitManagers();
            await StartManagers();
            await TransitionToAsync(GameState.StartMenu);
        }

        private Task InitManagers()
        {
            LightManager.Instance.ManagerInit();
            SoundManager.Instance.ManagerInit();
            InputManager.Instance.ManagerInit();
            GameSceneManager.Instance.ManagerInit();
            return Task.CompletedTask;
        }
        private async Task StartManagers()
        {
            // 時間がかかるのでシーン読み込みは先にやる
            Task loadBuckGorund = GameSceneManager.Instance.ManagerStartAsync();
            LightManager.Instance.ManagerStart();
            SoundManager.Instance.ManagerStart();
            InputManager.Instance.ManagerStart();
            await loadBuckGorund;
        }


        public async Task ShutdownGameAsync()
        {
            await TransitionToAsync(GameState.End);
        }

        public Task TransitionToDestroyAsyncInternal()
        {
            return DirectorDestroy();
        }

        protected override async Task DirectorDestroy()
        {
            // 先ずは開いているシーンを全部閉じて各シーンManagerのDirectorDestroyを動作させる
            Task unloadScenes = GameSceneManager.Instance.UnLoadAllScene();
            await unloadScenes;
            // SystemDirectorで管理しているDirectorを解放する
            LightManager.Instance.ManagerDestroy();
            SoundManager.Instance.ManagerDestroy();
            GameSceneManager.Instance.ManagerDestroy();
            // InputManagerだけは最後に解放する
            InputManager.Instance.ManagerDestroy();
        }

        protected override void DirectorRunTime()
        {
        }

        public async Task TransitionToAsync(GameState nextStateKey)
        {
            if (IsTransitioning) return;
            IsTransitioning = true;

            if (CurrentState != null)
            {
                await CurrentState.OnExitState();
            }

            if (StateMap.TryGetValue(nextStateKey, out var nextState))
            {
                CurrentState = nextState;
                await CurrentState.OnEnterState();
            }

            IsTransitioning = false;
        }
    }
}
