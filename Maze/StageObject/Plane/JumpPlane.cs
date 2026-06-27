using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using static MazeGame.MazeGameConstants;
using System.Linq;

namespace MazeGame
{
    public class JumpPlane : PlaneTrap
    {
        /* ジャンプ時パラメータ */
        [SerializeField] short damage = 200;
        [SerializeField] int impact = 300;
        [SerializeField] GameObject jumpparticle;
        [SerializeField] int removeMaterialIndex = 1;
        [SerializeField] SoundData jumpSound;

        private void Awake()
        {
            if (jumpparticle == null)
            {
                throw new InvalidOperationException("ジャンププレハブが未設定");
            }
        }

        private async UniTaskVoid WaitPaticleTime(GameObject effect)
        {
            float particleTime = effect.GetComponent<ParticleSystem>().main.duration;
            await UniTask.Delay((int)(particleTime/2f*1000f));

        }

        private void OnCollisionEnter(Collision collision)
        {
            if (endFlg) return;
            if (collision.gameObject.layer == LayerMask.NameToLayer(MazeGameConstants.PlayerConstants.Layer))
            {
                endFlg = true;
                SoundManager soundManager = SoundManager.Instance;
                if(soundManager != null)
                {
                    soundManager.RequestSe(jumpSound, this.transform.position);
                }
                GameObject effect = Instantiate(jumpparticle, collision.gameObject.transform.position, Quaternion.identity);
                effect.SetActive(true);
                WaitPaticleTime(effect).Forget();
                collision.rigidbody.AddForce(collision.transform.up * impact);
                collision.gameObject.GetComponent<PlayerController>().AddDamage(damage);
                RemoveMaterialAtIndex(removeMaterialIndex);
                WaitPaticleTime(effect).Forget();
                Destroy(effect);

            }
        }

        private void RemoveMaterialAtIndex(int index)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer == null) return;

            var materialList = renderer.materials.ToList();

            if (index >= 0 && index < materialList.Count)
            {
                // 指定した番号の要素を消す
                materialList.RemoveAt(index);
                renderer.materials = materialList.ToArray();
            }
        }

    }
}
