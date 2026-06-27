using System;
using UnityEngine;
using static MazeGame.MazeGameConstants;

namespace MazeGame
{
    public class JumpAttackController : MonoBehaviour
    {
        [SerializeField] private Collider rightCollider;
        [SerializeField] private Collider lehtCollider;
        [SerializeField] private GameObject SharpRockEffect;
        [SerializeField] short damage;
        private bool isHit;
        private bool isEnable;
        [SerializeField] private SoundData voiceSound;

        void JumpAttackVoice()
        {
            SoundManager soundManager = SoundManager.Instance;
            if (soundManager != null)
            {
                soundManager.RequestSe(voiceSound, this.transform.position);
            }
        }

        private void Awake()
        {
            if (rightCollider == null || lehtCollider == null)
            {
                throw new Exception("コライダーが設定されていません");
            }
            rightCollider.enabled = false;
            lehtCollider.enabled = false;
            isEnable = false;
        }

        public void EnableJumpAttackCollider()
        {
            rightCollider.enabled = true;
            lehtCollider.enabled = true;
            isHit = false;
            isEnable = true;
        }
        public void DisableJumpAttackCollider()
        {
            rightCollider.enabled = false;
            lehtCollider.enabled = false;
            isHit = true;
            isEnable = false;
            GameObject sharpRockEffect = Instantiate(SharpRockEffect);
            sharpRockEffect.transform.position = this.transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isEnable) return;
            if (isHit) return;
            if (other.CompareTag(MazeGameConstants.PlayerConstants.Tag))
            {
                isHit = true;
                other.gameObject.GetComponent<PlayerController>().AddDamage(damage);
            }
        }


    }

}
