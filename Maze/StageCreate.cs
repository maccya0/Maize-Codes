using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Collections;
using Unity.Jobs;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

namespace MazeGame
{
    public class StageCreate : MonoBehaviour
    {

        [SerializeField] private StageObjData stageObjData;
        [SerializeField] public NavMeshSurface navMesh;
        [SerializeField] private GameObject itemBox;
        [SerializeField][Range(0, 255)] private uint enemyRange = 192;
        [SerializeField] StageConstants StageConstants;

        private GameObject goalObject;
        private GameObject startObject;
        private GameObject[] checkPoints;

        /* 処理時共通データ */
        private int Size;
        private MazeConstants.MazeObjKinds[,] mazeData;
        private Maze maze;
        private GameObject rootObject;
        private Vector3 rootPos = new Vector3(MazeGameConstants.MazeConstants.rootX, MazeGameConstants.MazeConstants.rootY, MazeGameConstants.MazeConstants.rootZ);

        public void Init(GameObject _goalObject , GameObject _startObject , GameObject[] _checkPointObjects)
        {
            if (navMesh == null)
            {
                throw new InvalidOperationException("ターゲットが未設定");
            }
            goalObject = _goalObject;
            startObject = _startObject;
            checkPoints = _checkPointObjects;
            maze = Maze.Instance;

        }

        public void Begin()
        {
            ResetStage();

            //迷路生成データ取得
            Size = maze.GetStageSize();
            mazeData = maze.GetMazeData();

            //迷路生成
            CreateMazeStage();
        }

        private void ResetStage()
        {
            Size = 0;
            mazeData = null;
            navMesh.RemoveData();

            // ステージ削除
            if (rootObject != null)
            {
                DestroyImmediate(rootObject);
            }
            rootObject = new GameObject();
            rootObject.name = StageConstants.RootName;
            rootObject.transform.position = rootPos;

            // 迷路情報削除
            maze = Maze.Instance;

        }

        public void Destroy()
        {
            navMesh.RemoveData();
        }



        private void CreateMazeStage()
        {
            int totalCells = Size * Size;
            var stageConst = StageConstants;

            NativeArray<MazeConstants.MazeObjKinds> flattenedMazeData = new NativeArray<MazeConstants.MazeObjKinds>(totalCells, Allocator.TempJob);
            NativeArray<byte> randomValues = new NativeArray<byte>(totalCells, Allocator.TempJob);

            NativeArray<StageSpawnData> planeOutputs = new NativeArray<StageSpawnData>(totalCells, Allocator.TempJob);
            NativeArray<StageSpawnData> wallOutputs = new NativeArray<StageSpawnData>(totalCells, Allocator.TempJob);
            NativeArray<StageSpawnData> itemOutputs = new NativeArray<StageSpawnData>(totalCells, Allocator.TempJob);

            for (int cloop = 0; cloop < Size; cloop++)
            {
                for (int rloop = 0; rloop < Size; rloop++)
                {
                    int idx = cloop * Size + rloop;
                    flattenedMazeData[idx] = mazeData[cloop, rloop]; 
                }
            }

            MazeGenerationJob generationJob = new MazeGenerationJob
            {
                Size = Size,
                PosOffset = StageConstants.PosOffset,
                MazeDataFlattened = flattenedMazeData,
                RandomValues = randomValues,
                PlaneOutputs = planeOutputs,
                WallOutputs = wallOutputs,
                ItemOutputs = itemOutputs
            };

            JobHandle jobHandle = generationJob.Schedule(totalCells, 1);
            jobHandle.Complete();


            for (int i = 0; i < totalCells; i++)
            {
                int c = i / Size;
                int r = i % Size;

                // --- 床の生成 ---
                StageSpawnData pData = planeOutputs[i];
                GameObject plane;
                if (pData.spawnType == MazeConstants.MazeObjKinds.ETrapPath) // トラップ
                    plane = Instantiate(stageObjData.PlaneTrapList[(pData.prefabIndex % stageObjData.PlaneTrapList.Count)]);
                else
                    plane = Instantiate(stageObjData.PlanePrehab);

                plane.transform.position = pData.position;
                SetStageInfo(plane, c, r);

                // --- 壁の生成 ---
                StageSpawnData wData = wallOutputs[i];
                if (wData.spawnType != MazeConstants.MazeObjKinds.None)
                {
                    GameObject wall = null;
                    if (wData.spawnType == MazeConstants.MazeObjKinds.EUnBreakWall) // 不可壊
                    {
                        wall = Instantiate(stageObjData.UnBreakableWall);
                        wall.tag = "Indestructible";
                    }
                    else if (wData.spawnType == MazeConstants.MazeObjKinds.ETrapWall) // 通常（※実際はJob内で細分化可能）
                    {
                        wall = Instantiate(stageObjData.NormalWall);
                        wall.tag = MazeGameConstants.MazeConstants.wallTag;
                    }

                    if (wall != null)
                    {
                        wall.transform.position = wData.position;
                        wall.transform.rotation = wData.rotation;
                        SetStageInfo(wall, c, r);
                    }
                }
                // --- アイテムの生成 ---
                StageSpawnData iData = itemOutputs[i];
                if (iData.spawnType == MazeConstants.MazeObjKinds.EItem)
                {
                    GameObject item = Instantiate(itemBox);
                    item.transform.position = iData.position;
                    MoveCenterPosition(item);
                    item.transform.SetParent(rootObject.transform, false);
                }

                // 外周の透明壁もここで生成
                CreateAlphaWall(c, r);
            }
            flattenedMazeData.Dispose();
            randomValues.Dispose();
            planeOutputs.Dispose();
            wallOutputs.Dispose();
            itemOutputs.Dispose();

            CreateAlphaCeiling();   //透明な天井を生成
            CreateStageGimic(); //ステージギミックの生成
            navMesh.BuildNavMesh();  //ナビメッシュをビルドする

            // ナビメッシュエージェントのためにナビメッシュをビルドした後にする
            // 先ずはスポーン位置を決定する
            int num = 0;
            for (int cloop = 0; cloop < Size; cloop++)
            {
                for (int rloop = 0; rloop < Size; rloop++)
                {
                    if(mazeData[cloop, rloop] == MazeConstants.MazeObjKinds.EEnemyPos)
                    {
                        EnemyManager.Instance.RegisterSpownPoints(maze.stageObjects[cloop, rloop].transform.position);
                        num++;
                    }
                }
            }
        }

        private void CreateAlphaWall(int column, int row)
        {
            //四隅で生成
            if (row == 0 || row == Size - 1 || column == 0 || column == Size - 1)
            {
                GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                wall.GetComponent<Renderer>().enabled = false;
                wall.transform.localPosition = new Vector3(column * StageConstants.PosOffset, StageConstants.StageHeightLimit, row * StageConstants.PosOffset);
                wall.tag = "Indestructible";
                MoveCenterPosition(wall);
                wall.transform.SetParent(rootObject.transform,false);
            }
        }


        private void CreateAlphaCeiling()
        {
            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.GetComponent<Renderer>().enabled = false;
            wall.transform.localScale = new Vector3(Size, 1.0f, Size);
            wall.transform.localPosition = new Vector3((float)((Size - StageConstants.PosOffset) / 2), StageConstants.StageHeightLimit, (float)((Size - StageConstants.PosOffset) / 2));
            wall.tag = "Indestructible";
            MoveCenterPosition(wall);
            wall.transform.SetParent(rootObject.transform, false);
        }

        private void CreateStageGimic()
        {
            if (goalObject == null || startObject == null)
            {
                Debug.LogError("Goal or Start object is missing!");
                return;
            }

            var stageConst = StageConstants;

            for (int row = 0; row < Size - 1; row++)
            {
                if (mazeData[Size - 1, row] == MazeConstants.MazeObjKinds.EGoal)
                {
                    Vector3 goalPos = new Vector3(
                        (Size - 1) * stageConst.PosOffset + stageConst.GoalGimicOfsetHeight,
                        0,
                        (row + 1) * stageConst.PosOffset + stageConst.GoalGimicOfsetVertcal
                    );
                    SetupGimicTransform(goalObject, goalPos, 180f);
                    break;
                }
            }

            for (int row = 0; row < Size - 1; row++)
            {
                if (mazeData[0, row] == MazeConstants.MazeObjKinds.EStart)
                {
                    Vector3 startPos = new Vector3(
                        0 * stageConst.PosOffset - stageConst.StartGimicOfsetHeight,
                        0,
                        (row + 1) * stageConst.PosOffset - stageConst.StartGimicOfsetVertcal
                    );
                    SetupGimicTransform(startObject, startPos, 0f);
                    break;
                }
            }

            int cpIndex = 0;
            int mazeRate = maze.GetRate();

            for (int column = 0; column < Size - 1; column++)
            {
                if (cpIndex >= checkPoints.Length) break;

                if (mazeData[column, 0] == MazeConstants.MazeObjKinds.EChecPoint)
                {
                    Vector3 cpPos = new Vector3(
                        (column + 1) * stageConst.PosOffset + stageConst.CheckpointGimicOfsetHeight,
                        0,
                        0 * stageConst.PosOffset - stageConst.CheckpointGimicOfsetVertcal
                    );
                    SetupGimicTransform(checkPoints[cpIndex], cpPos, 270f);

                    cpIndex++;
                    column += mazeRate;
                }
            }

            // 🚩 奥のチェックポイント (Z = Size - 1 のライン)
            for (int column = 0; column < Size - 1; column++)
            {
                if (cpIndex >= checkPoints.Length) break;

                if (mazeData[column, Size - 1] == MazeConstants.MazeObjKinds.EChecPoint)
                {
                    Vector3 cpPos = new Vector3(
                        (column + 1) * stageConst.PosOffset - stageConst.CheckpointGimicOfsetHeight,
                        0,
                        (Size - 1) * stageConst.PosOffset + stageConst.CheckpointGimicOfsetVertcal
                    );
                    SetupGimicTransform(checkPoints[cpIndex], cpPos, 90f);

                    cpIndex++;
                    column += mazeRate;
                }
            }

            void SetupGimicTransform(GameObject obj, Vector3 localPos, float yRotation)
            {
                if (obj == null) return;
                obj.transform.position = localPos + rootPos;
                obj.transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
                MoveCenterPosition(obj);
            }
        }
        private void MoveCenterPosition(GameObject target)
        {
            if (target == null) return;
            Renderer renderer = target.GetComponent<Renderer>();
            if (renderer == null) return;

            Vector3 currentPivot = target.transform.position;
            Vector3 bottomCenter = new Vector3(renderer.bounds.center.x, renderer.bounds.min.y, renderer.bounds.center.z);
            Vector3 offset = currentPivot - bottomCenter;
            target.transform.position += offset;

        }

        private void SetStageInfo(GameObject stageObject ,int column, int row)
        {
            MoveCenterPosition(stageObject);
            stageObject.transform.SetParent(rootObject.transform,false);
            maze.stageObjects[column, row] = stageObject;
        }


    }
}
