using System;
using UnityEngine;
using static MazeGame.MazeGameConstants;

namespace MazeGame
{
    public class StrongAttackController : MonoBehaviour
    {
        [SerializeField] private Collider hitCollider;
        [SerializeField] short damage;
        private bool isHit;
        private bool isEnable;
        [SerializeField] private SoundData voiceSound;

        void StrongAttackVoice()
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

        public void EnableStrongAttackCollider()
        {
            hitCollider.enabled = true;
            isHit = false;
            isEnable = true;
            Debug.Log("enable Collider");
        }
        public void DisableStrongAttackCollider()
        {
            hitCollider.enabled = false;
            isHit = true;
            isEnable = false;
            Debug.Log("disable Collider");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isEnable) return;
            if (isHit) return;
            if (other.CompareTag(PlayerConstants.Tag))
            {
                isHit = true;
                other.gameObject.GetComponent<PlayerController>().AddDamage(damage);
            }
        }


    }

}
