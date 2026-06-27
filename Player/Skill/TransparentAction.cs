using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static MazeGame.MazeGameConstants;

namespace MazeGame
{
    public class TransparentAction : SkillAction
    {
        const string TransparentTag = "Transparent";

        public override void Init(PlayerController _playerController, InputSystem_Actions _actions)
        {
            base.Init(_playerController, _actions);
            actions.Player.Transparent.performed += OnTransparentPerformed;
        }

        public override void Begin()
        {
            base.Begin();
        }

        public override void Cleanup()
        {
            StopAllCoroutines();
            if (actions != null)
            {
                actions.Player.Transparent.performed -= OnTransparentPerformed;
            }
            actions = null;
        }

        public void OnTransparentPerformed(InputAction.CallbackContext context)
        {
            base.StartAction();
        }

        protected override IEnumerator ExecuteRoutine()
        {
            // スキル実行確定
            SkillExecute();
            particleObject = InstantiateAndDestroy(skillData.particle, transform.position, Quaternion.identity,skillData.sound);
            particleObject.transform.SetParent(transform);
            gameObject.tag = TransparentTag;
            float actionTime = Mathf.Max(0, skillData.actionTime);
            while (actionTime > 0)
            {
                actionTime -= Time.deltaTime;
                yield return null;
            }
            Destroy(particleObject);
            gameObject.tag = MazeGameConstants.PlayerConstants.Tag;
        }
    }

}

