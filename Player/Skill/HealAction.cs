using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MazeGame
{
    [RequireComponent(typeof(Transform))]
    public class HealAction : SkillAction
    {
        [SerializeField] private short HealValue = 300;

        protected  override void Awake()
        {
            base.Awake();
        }

        private void OnEnable()
        {
            actions.Player.Heal.performed += OnHealPerformed;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            if (actions != null)
            {
                actions.Player.Heal.performed -= OnHealPerformed;
            }
        }



        public void OnHealPerformed(InputAction.CallbackContext context)
        {
            base.StartAction();
        }

        protected override IEnumerator ExecuteRoutine()
        {
            // スキル実行確定
            SkillExecute();
            // パーティクルは常にプレイヤー位置に移動させる
            particleObject = InstantiateAndDestroy(skillData.particle, transform.position, Quaternion.identity,skillData.sound);
            particleObject.transform.SetParent(transform);
           
            float timer = skillData.actionTime;
            particleObject.gameObject.SetActive(true);
            while (timer > 0)
            {
                float diffTime = Time.deltaTime;
                timer -= diffTime;
                float healAmount = HealValue * diffTime / skillData.actionTime;
                playerController.HealHP((short)Mathf.Min(healAmount, short.MaxValue));
                yield return null;
            }
            Destroy(particleObject);
        }
    }
}
