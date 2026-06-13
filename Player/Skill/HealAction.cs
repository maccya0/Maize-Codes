using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MazeGame
{
    [RequireComponent(typeof(Transform))]
    public class HealAction : SkillAction
    {
        [SerializeField] private short HealValue = 300;


        public override void Init(PlayerController _playerController, InputSystem_Actions _actions)
        {
            base.Init(_playerController, _actions);
            actions.Player.Heal.performed += OnHealPerformed;
        }

        public override void Begin()
        {
            base.Begin();
            particleObject = null;
        }

        public override void Cleanup()
        {
            StopAllCoroutines();
            if (actions != null)
            {
                actions.Player.Heal.performed -= OnHealPerformed;
            }
            actions = null;
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
