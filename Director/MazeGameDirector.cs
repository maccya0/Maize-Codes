using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static MazeGame.MazeGameConstants.MazeConstants;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

namespace MazeGame
{
    public class MazeGameDirector : BaseDirector<GameDirector>
    {
        private StageGenerater StageGenerater;
        private PlayerGenerater PlayerGenerater;
        private UIGenerater UIGenerater;
        private GameSystemGenerater GameSystemGenerater;
        private SystemDirector SystemDirector;
        private CheckPointProgress CheckPointProgress;

        private bool isGamePlaying;
        public async Task Init()
        {
            isGamePlaying = false;
            SystemDirector = UnityEngine.Object.FindAnyObjectByType<SystemDirector>();
            if (SystemDirector == null)
            {
                throw new Exception("Not Find A SystemDirector");
            }
            DirectorInit();
            await DirectorStart();
            isGamePlaying = true;
        }

        public void Tick()
        {
            if (!isGamePlaying) return;
            DirectorRunTime();
        }

        public void Destroy()
        {
            DirectorDestroy();
        }

        protected override void DirectorInit()
        {
            StageGenerater = new StageGenerater(SystemDirector.GetLevelSelection());
            GameSystemGenerater = new GameSystemGenerater();
            PlayerGenerater = new PlayerGenerater();
            UIGenerater = new UIGenerater(SystemDirector.GetLevelSelection());
        }
        protected override async Task DirectorStart()
        {
            // 初期化関係
            await GameInit();

            // 開始関連
            await GameStart();
        }

        private Task GameInit()
        {
            // 初期化関連はゲーム全体→機能の順で設定していく
            // System系設定
            GameSystemGenerater.Init();
            // Stage系設定
            StageGenerater.Init();
            // Player系設定
            PlayerGenerater.Init();
            // UI系設定
            UIGenerater.Init();
            return Task.CompletedTask;
        }

        private Task GameStart()
        {
            // 開始関連はゲーム全体→機能の順で設定していく
            // System系設定
            GameSystemGenerater.Generated();
            var judger = GameSystemGenerater?.GetGameJudger();
            if (judger != null)
            {
                judger.OnGameFinished += HandleGameFinished;
            }
            // Stage系設定
            StageGenerater.Generated();
            // Player系設定
            PlayerGenerater.Generated();
            // UI系設定
            UIGenerater.Generated();
            return Task.CompletedTask;
        }

        protected override void DirectorRunTime()
        {
            // 更新関連はゲーム全体→機能の順で設定していく
            // System系更新
            GameSystemGenerater.Tick();
            var judger = GameSystemGenerater?.GetGameJudger();
            if (judger == null)
            {
                judger.OnGameFinished -= HandleGameFinished;
            }
            // Stage系更新
            StageGenerater.Tick();
            // Player系更新
            PlayerGenerater.Tick();
            // UI系更新
            UIGenerater.Tick();
        }

        private void HandleGameFinished(bool judge)
        {
            if (!isGamePlaying) return;
            isGamePlaying = false;
            var judger = GameSystemGenerater?.GetGameJudger();
            if (judger == null)
            {
                judger.OnGameFinished -= HandleGameFinished;
            }
            GameSystemGenerater.EndGame();
            StageGenerater.EndGame();
            PlayerGenerater.EndGame();
            UIGenerater.EndGame();
        }

        protected override Task DirectorDestroy()
        {
            // 削除処理は機能→ゲーム全体の順で設定していく
            // UI系削除
            UIGenerater.Destroy();
            // Player系削除
            PlayerGenerater.Destroy();
            // Stage系削除
            StageGenerater.Destroy();
            // System系削除
            GameSystemGenerater.Destroy();
            return Task.CompletedTask;
        }

    }
}
