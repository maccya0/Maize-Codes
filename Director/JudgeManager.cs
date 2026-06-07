using System;
using UnityEngine;
using UnityEngine.EventSystems;
using static MazeGame.MazeGameConstants.MazeConstants;

namespace MazeGame
{
    public class JudgeManager : MonoBehaviour
    {
        // 全体制御
        private bool isJudged;

        // ゴール関連
        [SerializeField] GoalController GoalController;
        
        // プレイヤー関連
        [SerializeField] PlayerController PlayerController;
        [SerializeField] int MaxDeathCount = 3;
        [SerializeField] MessageScrollManager messageScrollManager;
        [SerializeField] InputManager inputManager;
        [SerializeField] ImageTextAnimator resultWindow;
        [SerializeField] GameObject selectStageButton;
        [SerializeField] SoundData resultBgm;


        private int deathCount = 0;

        // 時間関連
        [SerializeField] GameTimeManager TimeManager;

        [SerializeField] GameDirector gameDirector;
        public event Action<GameState> SetState;

        private void Awake()
        {
            isJudged = false;
            GoalController.ReachedGoal += CheckGoalEvent;

            PlayerController.DiedEvent += CheckDeathEvent;
            TimeManager.TimeUpEvent += CheckTimeEvent;
        }
        private void OnDestroy()
        {
            GoalController.ReachedGoal -= CheckGoalEvent;
            PlayerController.DiedEvent -= CheckDeathEvent;
            TimeManager.TimeUpEvent -= CheckTimeEvent;
        }

        public void InitInfo()
        {
            deathCount = 0;
            isJudged = false;
        }

        private void CheckGoalEvent()
        {
            if (isJudged) return;
            if(gameDirector.IsAllCheckedPoints)
            {
                SetState.Invoke(GameState.Win);
                isJudged = gameDirector.IsAllCheckedPoints;
                messageScrollManager.EnqueueMessage("踏破した");
                const bool clear = true;
                PlayResultAnimation(clear);
            }
            else
            {
                messageScrollManager.EnqueueMessage("条件を満たせていない");
            }
        }

        private void CheckDeathEvent()
        {
            if (isJudged) return;
            deathCount++;
            if (deathCount >= MaxDeathCount)
            {
                SetState.Invoke(GameState.Lose);
                const bool failed = false;
                PlayResultAnimation(failed);
            }
        }
        private void CheckTimeEvent()
        {
            if (isJudged) return;
            SetState.Invoke(GameState.Lose);
            const bool failed = false;
            PlayResultAnimation(failed);
        }

        private void PlayResultAnimation(bool isClear)
        {
            inputManager.ChangeInputModePlayerToUI();
            UIWindowManager.Instance.ActiveResultWindow();
            EventSystem.current.SetSelectedGameObject(selectStageButton.gameObject);
            const bool requestFade = true;
            SoundManager.Instance.StartBgm(resultBgm, requestFade);
            if (isClear)
            {
                StartCoroutine(resultWindow.PlayClearAnimation());
            }
            else
            {
                StartCoroutine(resultWindow.PlayFinishAnimation());
            }
        }





    }
}
