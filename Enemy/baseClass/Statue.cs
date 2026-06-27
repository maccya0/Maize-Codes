using UnityEngine;
using System.Collections;
using static MazeGame.MazeGameConstants;

namespace MazeGame
{
    public abstract class Statue : MonoBehaviour
    {
        [Header("Statue Settings")]
        [SerializeField] protected float RoundSpeed;  //回転速度
        [SerializeField] protected float SearchRnage; //サーチ範囲
        private bool isRecast;
        [SerializeField] private float RecastTime = 3f;
        [SerializeField] private SoundData findSound;
        [Header("Debug Settings")]
        [SerializeField] private float rayDuration = 0.1f;


        protected virtual void Awake()
        {
            // Y軸で初期回転を360°の中から選択する
            this.gameObject.transform.Rotate(new Vector3(0, UnityEngine.Random.Range(0, 359), 0));
            isRecast = false;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (isRecast) return;
            RotateStatue();
            CheckRayPos();
        }

        private void RotateStatue()
        {
            //Δタイムだけ回転する
            this.gameObject.transform.Rotate(new Vector3(0, RoundSpeed * Time.deltaTime, 0));
        }

        private IEnumerator ReCast()
        {
            isRecast = true;
            yield return new WaitForSeconds(RecastTime);
            isRecast = false;
        }

        protected abstract void ExecuteStatueSkill(GameObject gameObject);
        private void CheckRayPos()
        {
            Vector3 rayPos = this.transform.position;
            rayPos.y = PlayerConstants.Height;
            Ray ray = new Ray(rayPos, this.transform.forward);
            RaycastHit hit;
            int layerMask = LayerMask.GetMask("Player", "Obstacle");
            if (Physics.Raycast(ray, out hit, SearchRnage, layerMask))
            {
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, rayDuration);

                if (hit.collider.tag == PlayerConstants.Tag)
                {
                    SoundManager.Instance.RequestSe(findSound,transform.position);
                    ExecuteStatueSkill(hit.collider.gameObject);
                    StartCoroutine(ReCast());
                }
            }

        }

    }

}
