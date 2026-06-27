using System;
using UnityEngine;
using static MazeGame.MazeGameConstants;

namespace MazeGame
{
    public class LightAttackController : MonoBehaviour
    {
        [SerializeField] private Collider hitCollider;
        [SerializeField] short damage;
        private bool isHit;
        private bool isEnable;
        [SerializeField] private SoundData voiceSound;

        void LightAttackVoice()
        {
            SoundManager soundManager = SoundManager.Instance;
            if (soundManager != null)
            {
                soundManager.RequestSe(voiceSound, this.transform.position);
            }
        }

        private void Awake()
        {
            if (hitCollider == null)
            {
                throw new Exception("コライダーが設定されていません");
            }
            hitCollider.enabled = false;
            isEnable = false;
        }

        public void EnableLightAttackCollider()
        {
            hitCollider.enabled = true;
            isHit = false;
            isEnable = true;
        }
        public void DisableLightAttackCollider()
        {
            hitCollider.enabled = false;
            isHit = true;
            isEnable = false;
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
