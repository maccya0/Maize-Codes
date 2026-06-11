//using System;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using static MazeGame.MazeGameConstants.MazeConstants;

//namespace MazeGame
//{
//    public class JudgeManager : BaseManager<JudgeManager>
//    {
//        // 全体制御
//        private bool isJudged;
//        private int deathCount;

//        // ゴール関連
//        [SerializeField] GoalController GoalController;
//        [SerializeField] ImageTextAnimator resultAnimation;

//        // プレイヤー関連
//        [SerializeField] PlayerController PlayerController;
//        [SerializeField] int MaxDeathCount = 3;
//        [SerializeField] SoundData resultBgm;

//        public event Action<GameState> SetState;

//        protected override void Awake()
//        {
//            base.Awake();
//            if(Instance !=this) return;
//        }


//        public override void ManagerInit()
//        {
//            base.ManagerInit();
//            GoalController.ReachedGoal += CheckGoalEvent;
//            PlayerController.DiedEvent += CheckDeathEvent;
//            TimeManager.Instance.TimeUpEvent += CheckTimeEvent;
//        }

//        public override void ManagerStart()
//        {
//            base.ManagerStart();
//            isJudged = false;
//            deathCount =  0;
//        }

//        public override void ManagerDestroy()
//        {
//            base.ManagerDestroy();
//            if(Instance !=this) return;
//            GoalController.ReachedGoal -= CheckGoalEvent;
//            PlayerController.DiedEvent -= CheckDeathEvent;
//            TimeManager.Instance.TimeUpEvent -= CheckTimeEvent;
//        }

//        private void CheckGoalEvent()
//        {
//            if (isJudged) return;
//            if(GameDirector.Instance.IsAllCheckedPoints)
//            {
//                SetState.Invoke(GameState.Win);
//                isJudged = GameDirector.Instance.IsAllCheckedPoints;
//                MessageScrollManager.Instance.EnqueueMessage("踏破した");
//                const bool clear = true;
//                PlayResultAnimation(clear);
//            }
//            else
//            {
//                MessageScrollManager.Instance.EnqueueMessage("条件を満たせていない");
//            }
//        }

//        private void CheckDeathEvent()
//        {
//            if (isJudged) return;
//            deathCount++;
//            if (deathCount >= MaxDeathCount)
//            {
//                SetState.Invoke(GameState.Lose);
//                const bool failed = false;
//                PlayResultAnimation(failed);
//            }
//        }
//        private void CheckTimeEvent()
//        {
//            if (isJudged) return;
//            SetState.Invoke(GameState.Lose);
//            const bool failed = false;
//            PlayResultAnimation(failed);
//        }

//        private void PlayResultAnimation(bool isClear)
//        {
//            InputManager.Instance.ChangeInputModePlayerToUI();
//            UIWindowManager.Instance.ActiveResultWindow();
//            const bool requestFade = true;
//            SoundManager.Instance.StartBgm(resultBgm, requestFade);
//            if (isClear)
//            {
//                StartCoroutine(resultAnimation.PlayClearAnimation());
//            }
//            else
//            {
//                StartCoroutine(resultAnimation.PlayFinishAnimation());
//            }
//        }
//    }
//}
