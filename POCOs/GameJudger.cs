using System;
using UnityEngine;
using UnityEngine.EventSystems;
using static MazeGame.MazeGameConstants.MazeConstants;

namespace MazeGame
{
    public class GameJudger : IDisposable
    {
        // 全体制御
        private bool isJudged;
        private int deathCount;
        private bool isChecked;

        // ゴール/プレイヤー関連
        private readonly int maxDeathCount;
        private readonly GoalController goalController;
        private readonly PlayerController playerController;

        public event Action<bool> OnGameFinished;

        public GameJudger(GoalController goal, PlayerController player, int maxDeath)
        {
            goalController = goal;
            playerController = player;
            maxDeathCount = maxDeath;

            // イベントの購読
            if (goalController != null) goalController.ReachedGoal += CheckGoalEvent;
            if (playerController != null) playerController.DiedEvent += CheckDeathEvent;
            if (TimeManager.Instance != null) TimeManager.Instance.TimeUpEvent += CheckTimeEvent;
        }

        public void Start()
        {
            isJudged = false;
            deathCount = 0;
            isChecked = false;
        }
        public void Dispose()
        {
            if (goalController != null) goalController.ReachedGoal -= CheckGoalEvent;
            if (playerController != null) playerController.DiedEvent -= CheckDeathEvent;
            if (TimeManager.Instance != null) TimeManager.Instance.TimeUpEvent -= CheckTimeEvent;
        }

        private void CheckGoalEvent()
        {
            if (isJudged) return;
            if(isChecked)
            {
                isJudged = true;
                MessageScrollManager.Instance.EnqueueMessage("踏破した");
                OnGameFinished?.Invoke(true);
            }
            else
            {
                MessageScrollManager.Instance.EnqueueMessage("条件を満たせていない");
            }
        }

        private void CheckDeathEvent()
        {
            if (isJudged) return;
            deathCount++;
            if (deathCount >= maxDeathCount)
            {
                isJudged = true;
                OnGameFinished?.Invoke(false);
            }
        }
        private void CheckTimeEvent()
        {
            if (isJudged) return;
            isJudged = true;
            OnGameFinished?.Invoke(false);
        }
        public void CheckdAllPoints()
        {
            isChecked = true;
        }
    }
}
