using UnityEngine;
using static MazeGame.MazeGameConstants.MazeConstants;
using static MazeGame.MazeGameConstants;
using System;
using Cysharp.Threading.Tasks;
using System.Linq;

namespace MazeGame
{
    public class BombBlock : MonoBehaviour
    {
        [SerializeField] private float rayDistance = 1.0f;   // Rayの距離
        [SerializeField] private float impactForce = 3000.0f; // 爆発威力
        [SerializeField] private short damage = 500; //  ダメージ
        [SerializeField] public GameObject BombEffect;
        [SerializeField] int removeMaterialIndex = 1;
        [SerializeField] private SoundData bombSound;
        private Vector3 Directiom; // Rayの方向(BombDirect)と紐づく
        /* 動作制御用 */
        private bool runFlg = false; // 実行フラグ

        public void Awake()
        {
            if (BombEffect == null)
            {
                throw new InvalidOperationException("ボムプレハブが未設定");
            }
        }

        // 爆発検知の方向を設定する
        public void SetBombInfo(Direct _direct)
        {
            runFlg = false;
            switch (_direct)
            {
                case Direct.North:
                    Directiom = Vector3.forward;
                    break;
                case Direct.East:
                    Directiom = Vector3.right;
                    break;
                case Direct.South:
                    Directiom = Vector3.back;
                    break;
                case Direct.West:
                    Directiom =Vector3.left;
                    break;
                default:
                    //  基本的にここには来ないが念のため設定する
                    Directiom = Vector3.zero;
                    break;

            }

        }

        //  Update is called once per frame
        void Update()
        {
            if (runFlg) return;
            CheckRayhit();
        }

        private async UniTaskVoid PlayEffect(GameObject bomb)
        {
            float particleTime = bomb.GetComponent<ParticleSystem>().main.duration;
            await UniTask.Delay((int)particleTime*1000);
            Destroy(bomb);
        }

        private void CheckRayhit()
        {
            // 壁の高さを基準にするとY座標が高いので調整する
            Vector3 rayPos = this.transform.position;
            rayPos.y = PlayerConstants.Height;
            Ray ray = new Ray(rayPos, Directiom);
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.yellow, 0, false);
            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer(PlayerConstants.Layer))
                {
                    runFlg = true;
                    // Rayから対象へのベクトルを作成して正規化する事で向きにする
                    // Y軸が高すぎるため、ーY軸側に飛んでしまう
                    // 後で直す
                    Vector3 direct = Directiom;
                    hit.collider.gameObject.GetComponent<PlayerController>().AddDamage(damage);
                    Vector3 explosionPos = this.transform.position;
                    explosionPos.y = PlayerConstants.Height;
                    SoundManager soundManager = SoundManager.Instance;
                    if (soundManager != null)
                    {
                        soundManager.RequestSe(bombSound, explosionPos);
                    }
                    GameObject effect = Instantiate(BombEffect, explosionPos, Quaternion.identity);
                    effect.SetActive(true);
                    hit.collider.attachedRigidbody.AddExplosionForce(impactForce, explosionPos, rayDistance * 3);
                    PlayEffect(effect).Forget();
                    RemoveMaterialAtIndex(removeMaterialIndex);
                }
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
