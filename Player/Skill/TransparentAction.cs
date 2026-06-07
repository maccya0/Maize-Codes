using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static MazeGame.MazeGameConstants;

namespace MazeGame
{
    public class TransparentAction : SkillAction
    {
        protected override void Awake()
        {
            base.Awake();
        }

        private void OnEnable()
        {
            actions.Player.Transparent.performed += OnTransparentPerformed;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            if(actions != null)
            {
                actions.Player.Transparent.performed -= OnTransparentPerformed;
            }
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
            gameObject.tag = "Transparent";
            float actionTime = Mathf.Max(0, skillData.actionTime);
            while (actionTime > 0)
            {
                actionTime -= Time.deltaTime;
                yield return null;
            }
            Destroy(particleObject);
            gameObject.tag = PlayerConstants.Tag;
        }
    }

}

