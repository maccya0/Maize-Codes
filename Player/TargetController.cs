using System;
using UnityEngine;


namespace MazeGame
{
    public class TargetController : MonoBehaviour
    {
        [SerializeField] private  TargetData targetData;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float MaxDistance = 10f;
        private GameObject target;
        private GameObject preTarget;
        public event Action<GameObject> OnChangeEvent;

        private void Awake()
        {
            target = null;
            preTarget = null;
        }

        private void Update()
        {
            UpdateTargeting();
        }

        private void UpdateTargeting()
        {
            // 画面中央からRayを飛ばす
            Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;

            GameObject newTarget = null;


            // ToDo：レイヤーマスクは対象を整理してから再設定
            int layerMask = LayerMask.GetMask("Default", "Obstacle");

            if (Physics.Raycast(ray, out hit, MaxDistance, layerMask))
            {
                GameObject hitObj = hit.collider.gameObject;

                // TargetDataに含まれるタグかどうかチェック
                if (targetData.TargetList.Contains(hitObj.tag))
                {
                    newTarget = hitObj;
                }
            }

            // ターゲットが変わった場合のみ更新
            if (newTarget != preTarget)
            {
                ChangeTarget(newTarget);
                preTarget = newTarget;
            }

        }

        private void ChangeTarget(GameObject newTarget)
        {
            // アウトラインの更新
            SetOutLine(target, false);
            target = newTarget;
            SetOutLine(target, true);
            OnChangeEvent?.Invoke(target);
        }

        private void SetOutLine(GameObject target, bool state)
        {
            if(target == null) return;

            //ToDo: コンポーネントからアウトレットラインをON / OFFする
            if (target.TryGetComponent<Outline>(out var OutLine))
            {
                if (state)
                {
                    OutLine.OutlineMode = Outline.Mode.OutlineAll;
                }
                else
                {
                    OutLine.OutlineMode = Outline.Mode.OutlineHidden;
                }
            }
        }

        public bool IsTargeting() => target != null;
        public GameObject CurrentTarget() => target;
    }

}
