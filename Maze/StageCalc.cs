using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace MazeGame
{
    public struct StageSpawnData
    {
        public Vector3 position;
        public Quaternion rotation;

        public MazeConstants.MazeObjKinds spawnType;

        public int prefabIndex;
        public MazeConstants.Direct bombDirection;
    }

    public struct MazeGenerationJob : IJobParallelFor
    {
        [ReadOnly] public int Size;
        [ReadOnly] public float PosOffset;
        [ReadOnly] public NativeArray<MazeConstants.MazeObjKinds> MazeDataFlattened;
        [ReadOnly] public NativeArray<byte> RandomValues;

        public NativeArray<StageSpawnData> PlaneOutputs;
        public NativeArray<StageSpawnData> WallOutputs;
        public NativeArray<StageSpawnData> ItemOutputs;

        public void Execute(int index)
        {
            int column = index / Size;
            int row = index % Size;
            byte rand = RandomValues[index];

            MazeConstants.MazeObjKinds kind = MazeDataFlattened[index];
            Vector3 pos = new Vector3(column * PosOffset, 0, row * PosOffset);

            // --- ① 床（Plane）の計算 ---
            StageSpawnData planeData = new StageSpawnData
            {
                position = pos,
                rotation = Quaternion.identity,
                spawnType = MazeConstants.MazeObjKinds.EPath
            };

            if (kind == MazeConstants.MazeObjKinds.ETrapPath)
            {
                planeData.spawnType = MazeConstants.MazeObjKinds.ETrapPath;
                planeData.prefabIndex = rand;
            }
            PlaneOutputs[index] = planeData;

            // --- ② 壁（Wall）の計算 ---
            StageSpawnData wallData = new StageSpawnData { position = Vector3.zero, spawnType = MazeConstants.MazeObjKinds.None };

            bool isWall = (kind == MazeConstants.MazeObjKinds.EBreakWall ||
                           kind == MazeConstants.MazeObjKinds.ETrapWall ||
                           kind == MazeConstants.MazeObjKinds.EUnBreakWall);

            if (isWall)
            {
                wallData.position = pos;
                wallData.rotation = Quaternion.identity;

                // 外周チェック
                if (row == 0 || row == Size - 1 || column == 0 || column == Size - 1)
                {
                    wallData.spawnType = MazeConstants.MazeObjKinds.EUnBreakWall;
                }
                else
                {
                    wallData.spawnType = kind;
                }
            }
            WallOutputs[index] = wallData;

            // --- ③ アイテムの計算 ---
            StageSpawnData itemData = new StageSpawnData { spawnType = MazeConstants.MazeObjKinds.None };
            if (kind == MazeConstants.MazeObjKinds.EItem)
            {
                itemData.position = pos;
                itemData.rotation = Quaternion.identity;
                itemData.spawnType = MazeConstants.MazeObjKinds.EItem;
            }
            ItemOutputs[index] = itemData;
        }
    }
}