using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MazeGame
{
    public class SlipThroughAction : SkillAction
    {
        private GameObject target;
        private GameObject backParticleObject;
        [SerializeField] float rayDistance = 3.0f;
        [SerializeField] TargetData targetData;

        enum WallType
        {
            None,
            LeftAndRight,
            BeforeAndAfter,
        }

        public override void Init(PlayerController _playerController, InputSystem_Actions _actions)
        {
            base.Init(_playerController, _actions);
            if (rayDistance == 0)
            {
                throw new InvalidOperationException("1以上の値が未設定");
            }
            if (targetData == null)
            {
                throw new InvalidOperationException("ターゲットが未設定");
            }
            actions.Player.SlipThrough.performed += OnSlipThroughPerformed;
        }

        public override void Begin()
        {
            base.Begin();
            target = null;
            backParticleObject = null;
        }

        public override void Cleanup()
        {
            StopAllCoroutines();
            if (actions != null)
            {
                actions.Player.SlipThrough.performed -= OnSlipThroughPerformed;
            }
            actions = null;
        }

        private void OnSlipThroughPerformed(InputAction.CallbackContext context)
        {
            base.StartAction();
        }

        protected override bool CanExecuteCustom()
        {
            target = GetTarget();
            /* あたり始めのオブジェクトを取得する */
            if (target != null && targetData.TargetList.Contains(target.tag))
            {
                WallType wallType = IsWall();
                switch(wallType)
                {
                    case WallType.None:
                        return false;
                    case WallType.BeforeAndAfter:
                    case WallType.LeftAndRight: 
                        return true;
                }
            }
            return false;
        }


        private WallType IsWall()
        {
            Maze maze = Maze.Instance;
            int column = 0, row = 0;
            maze.GetObjectPos(target.gameObject, ref column, ref row);
            
            // 前後が壁かどうか判定
            bool beforePos = (maze.GetStageinfo(column - 1, row) == MazeConstants.MazeObjKinds.EBreakWall) || (maze.GetStageinfo(column - 1, row) == MazeConstants.MazeObjKinds.EBreakWall);
            bool afterPos = (maze.GetStageinfo(column + 1, row) == MazeConstants.MazeObjKinds.EBreakWall) || (maze.GetStageinfo(column + 1, row) == MazeConstants.MazeObjKinds.EBreakWall);
            bool rightPos = (maze.GetStageinfo(column, row - 1) == MazeConstants.MazeObjKinds.EBreakWall) || (maze.GetStageinfo(column, row - 1) == MazeConstants.MazeObjKinds.EBreakWall);
            bool leftPos = (maze.GetStageinfo(column, row + 1) == MazeConstants.MazeObjKinds.EBreakWall) || (maze.GetStageinfo(column, row + 1) == MazeConstants.MazeObjKinds.EBreakWall);
            if (beforePos && afterPos)
            {
                // 前後にいる
                return WallType.BeforeAndAfter;

            }
            else if (rightPos && leftPos)
            {
                // 左右に居る
                return WallType.LeftAndRight;
            }
            else
            {
                // 設置できない
                return WallType.None;
            }

        }


        private void SetWarpPoint(Vector3 frontParticlePos, Vector3 backParticlePos ,float yawAngle)
        {
            // パーティクルが必ず正面になるように調整して配置
            frontParticlePos.y = MazeGameConstants.PlayerConstants.Height;
            backParticlePos.y = MazeGameConstants.PlayerConstants.Height;

            particleObject = Instantiate(skillData.particle, frontParticlePos, Quaternion.identity);
            particleObject.transform.Rotate(new Vector3(0, yawAngle, 0), Space.World);
            backParticleObject = Instantiate(skillData.particle, backParticlePos, Quaternion.identity);
            backParticleObject.transform.Rotate(new Vector3(0, yawAngle, 0), Space.World);

            // 柱を挟んで前後にワープポイントを設置
            particleObject.GetComponent<WarpController>().SetWarpPoint(backParticleObject);
            Vector3 tempPos;
            tempPos = particleObject.transform.position;
            particleObject.transform.position = tempPos;
            backParticleObject.GetComponent<WarpController>().SetWarpPoint(particleObject);
            tempPos = backParticleObject.transform.position;
            backParticleObject.transform.position = tempPos;
            particleObject.gameObject.SetActive(true);
            backParticleObject.gameObject.SetActive(true);
        }

        protected override IEnumerator ExecuteRoutine()
        {
            // スキル実行確定
            SkillExecute();
            // 事前の判定で必ず前後になる
            WallType wallType = IsWall();
            Vector3 frontParticlePos = target.transform.position;
            Vector3 backParticlePos = target.transform.position;
            float yawAngle;
            if (wallType == WallType.LeftAndRight)
            {
                // 前後にいる
                float size = target.GetComponent<Renderer>().bounds.size.z;
                frontParticlePos.x += size;
                backParticlePos.x -= size;
                yawAngle = 90.0f;

            }
            else
            {
                // 左右に居る
                float size = target.GetComponent<Renderer>().bounds.size.x;
                frontParticlePos.z += size;
                backParticlePos.z -= size;
                yawAngle = 0.0f;

            }

            SetWarpPoint(frontParticlePos, backParticlePos, yawAngle);
            StartSe(skillData.sound, this.transform.position);
            float actionTime = Mathf.Max(0, skillData.actionTime);

            yield return new WaitForSeconds(actionTime);

            if (particleObject != null) Destroy(particleObject);
            if (backParticleObject != null) Destroy(backParticleObject);
        }

        private GameObject GetTarget()
        {
            Vector3 rayPos = this.transform.position;
            Ray ray = new Ray(rayPos, this.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                return hit.collider.gameObject;
            }
            else
            {
                return null;
            }
        }
    }

}
