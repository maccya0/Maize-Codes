using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MazeGame
{
    public class DestroyObjAction : SkillAction
    {
        [SerializeField] TargetData targetData;
        private GameObject preHitObject;
        [SerializeField] private TargetController targetController;
        [SerializeField] ChargeGauge chargeGauge;
        private Coroutine coroutine;

        public override void Init(PlayerController _playerController, InputSystem_Actions _actions)
        {
            base.Init(_playerController, _actions);
            actions.Player.Destroy.performed += OnDestroyPerformed;
            actions.Player.Destroy.canceled += OnDestroyCanceled;
        }

        public override void Begin()
        {
            base.Begin();
            preHitObject = null;
            particleObject = null;
        }

        public override void Cleanup()
        {
            StopAllCoroutines();
            if (actions != null)
            {
                actions.Player.Destroy.performed -= OnDestroyPerformed;
                actions.Player.Destroy.canceled -= OnDestroyCanceled;
            }
            actions = null;
        }


        public void OnDestroyPerformed(InputAction.CallbackContext context)
        {
            if(isReady)
            {
                preHitObject = targetController.CurrentTarget();
            }
            base.StartAction();
        }

        protected override bool CanExecuteCustom()
        {
            return targetController.IsTargeting();  
        }

        protected override IEnumerator ExecuteRoutine()
        {
            float waitTime = Mathf.Max(0, skillData.actionTime);
            GameObject target = null;
            while (waitTime > 0)
            {
                //一定時間ターゲティングする
                target = JudgeDestroy();
                if (target != null)
                {
                    waitTime -= Time.deltaTime;
                    float rate = waitTime / skillData.actionTime;
                    chargeGauge.SetGauge(rate);
                    Debug.Log(waitTime);
                    yield return null;
                }
                else
                {
                    //アクション中にターゲットを見失うと中止する
                    chargeGauge.SetGauge(0.0f);
                    preHitObject = null;
                    yield break;
                }
            }
            // スキル実行確定
            SkillExecute();
            chargeGauge.SetGauge(0.0f);
            Vector3 actionPos = target.transform.position;
            InstantiateAndDestroy(skillData.particle, actionPos, Quaternion.identity,skillData.sound);
            Destroy(target);
            preHitObject = null;
        }

        private GameObject JudgeDestroy()
        {

            GameObject target = targetController.CurrentTarget();
            //前回のゲームオブジェクトと違うオブジェクトを捉えた場合又は壁以外を捉えたとき
            if (target == preHitObject && targetData.TargetList.Contains(target.tag))
            {
                preHitObject = target;
                return target;
            }
            else
            {
                return null;
            }
        }


        public void OnDestroyCanceled(InputAction.CallbackContext context)
        {
            StopAction();
        }
    }

}

