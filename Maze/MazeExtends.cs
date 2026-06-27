using System.Collections.Generic;
using Unity.Android.Gradle;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace MazeGame
{
    public class MazeExtends : DiggingHoleMaze
    {

        // 高速化の為にひとまとまりにする
        struct MakeMazeData
        {
            public int trapNum;
            public int enemyNum;
            public int itemNum;
            public int rate;
        }

        private MakeMazeData makeMazeData;

        private MazeConstants.MazeObjKinds[,] extendMaze; // 拡張した迷路

        //迷路生成データセット
        public void SetMazeData(int _size, int _tarapNum, int _enemyNum, int rate, int _checkPointNum, int _itemNum)
        {
            MazeConstants.MazeObjKinds[,] tempExtendMaze;
            base.SetMazeData(_size);
            tempExtendMaze = base.CreateMaze();

            makeMazeData.trapNum = _tarapNum;
            makeMazeData.enemyNum = _enemyNum;
            makeMazeData.rate = rate;
            this.extendMaze = new MazeConstants.MazeObjKinds[base.size * rate, base.size * rate];
            makeMazeData.itemNum = _itemNum;

            ExtendMaze(tempExtendMaze);
            SetGoalPos();
            SetStartPos();
            SetCheckPoint(_checkPointNum);

            SetupMazeObjects();
        }

        public int GetStageSize()
        {
            return base.size * makeMazeData.rate;
        }

        public MazeConstants.MazeObjKinds[,] GetMazeData()
        {
            return extendMaze;
        }
        private void SetGoalPos()
        {
            int stageSize = GetStageSize();
            // ループで計算をさせないためにループ外で予め計算しておく
            int stageSize_1 = stageSize - 1;
            int stageSize_2 = stageSize - 2;

            // 角以外から選ぶ
            // メモリが無駄なので固定じゃない方を覚える
            List<int> goalPos = new List<int>();
            for (int x = stageSize_2; x >1; x--)
            {
                if ( (extendMaze[stageSize_1, x-1] == MazeConstants.MazeObjKinds.EUnBreakWall) &&
                     (extendMaze[stageSize_2, x-1] == MazeConstants.MazeObjKinds.EPath)&&
                     (extendMaze[stageSize_1, x] == MazeConstants.MazeObjKinds.EUnBreakWall) &&
                     (extendMaze[stageSize_2, x] == MazeConstants.MazeObjKinds.EPath)&&
                     (extendMaze[stageSize_1, x+1] == MazeConstants.MazeObjKinds.EUnBreakWall) &&
                     (extendMaze[stageSize_2, x+1] == MazeConstants.MazeObjKinds.EPath))
                {
                    goalPos.Add((int)x);
                }
            }

            int randamPos = UnityEngine.Random.Range(0, goalPos.Count);
            extendMaze[(int)stageSize_1, (int)goalPos[randamPos] - 1] = MazeConstants.MazeObjKinds.EGoal;
            extendMaze[(int)stageSize_1, (int)goalPos[randamPos]] = MazeConstants.MazeObjKinds.EGoal;
            extendMaze[(int)stageSize_1, (int)goalPos[randamPos] + 1] = MazeConstants.MazeObjKinds.EGoal;

        }
        private void SetStartPos()
        {
            // 角以外から選ぶ
            // メモリが無駄なので固定じゃない方を覚える
            List<int> startPos = new List<int>();
            for (int x = 2; x < GetStageSize() - 2; x++)
            {
                if ((extendMaze[0, x - 1] == MazeConstants.MazeObjKinds.EUnBreakWall) &&
                     (extendMaze[1, x - 1] == MazeConstants.MazeObjKinds.EPath) &&
                     (extendMaze[0, x ] == MazeConstants.MazeObjKinds.EUnBreakWall) &&
                     (extendMaze[1, x ] == MazeConstants.MazeObjKinds.EPath) &&
                     (extendMaze[0, x + 1] == MazeConstants.MazeObjKinds.EUnBreakWall) &&
                     (extendMaze[1, x + 1] == MazeConstants.MazeObjKinds.EPath))
                {
                    startPos.Add((int)x);
                }
            }
            int randamPos = UnityEngine.Random.Range(0, startPos.Count);
            extendMaze[0, (int)startPos[randamPos] - 1] = MazeConstants.MazeObjKinds.EStart;
            extendMaze[0, (int)startPos[randamPos]] = MazeConstants.MazeObjKinds.EStart;
            extendMaze[0, (int)startPos[randamPos] + 1] = MazeConstants.MazeObjKinds.EStart;

        }
        private void SetCheckPoint(int checkPointNum)
        {
            int stageSize = GetStageSize();
            // ループで計算をさせないためにループ外で予め計算しておく
            int stageSize_1 = stageSize - 1;
            int stageSize_2 = stageSize - 2;
            // 角以外から選ぶ
            // 2列から選びたいのでVector2Int
            List<Vector2Int> checkpointList = new List<Vector2Int>();

            for (int y = 2; y < GetStageSize() - 2; y++)
            {
                if ((extendMaze[y - 1, 0] == MazeConstants.MazeObjKinds.EUnBreakWall) &&
                    (extendMaze[y - 1, 1] == MazeConstants.MazeObjKinds.EPath) &&
                    (extendMaze[y, 0] == MazeConstants.MazeObjKinds.EUnBreakWall) &&
                    (extendMaze[y, 1] == MazeConstants.MazeObjKinds.EPath) &&
                    (extendMaze[y + 1, 0] == MazeConstants.MazeObjKinds.EUnBreakWall) &&
                    (extendMaze[y + 1, 1] == MazeConstants.MazeObjKinds.EPath))
                {
                    checkpointList.Add(new Vector2Int(y, 0));
                }
                if ((extendMaze[y - 1, stageSize_1] == MazeConstants.MazeObjKinds.EUnBreakWall) &&
                    (extendMaze[y - 1, stageSize_2] == MazeConstants.MazeObjKinds.EPath) &&
                    (extendMaze[y, stageSize_1] == MazeConstants.MazeObjKinds.EUnBreakWall) &&
                    (extendMaze[y, stageSize_2] == MazeConstants.MazeObjKinds.EPath) &&
                    (extendMaze[y + 1, stageSize_1] == MazeConstants.MazeObjKinds.EUnBreakWall) &&
                    (extendMaze[y + 1, stageSize_2] == MazeConstants.MazeObjKinds.EPath))
                {
                    checkpointList.Add(new Vector2Int(y, stageSize_2));
                }
            }

            checkpointList.Shuffle();
            // ステージを三分割した距離は離すペアを探す
            int minDist = stageSize / checkPointNum;
            List<Vector2Int> candidates = new List<Vector2Int>();
            for (int i = 0; i < checkpointList.Count; i++)
            {
                candidates.Add(checkpointList[i]);
                for (int j = i; j < checkpointList.Count; j++)
                {
                    Vector2Int candidatePos = checkpointList[j];
                    if(IsFarEnough(candidatePos,candidates,minDist))
                    {
                        candidates.Add(candidatePos);
                    }
                    if(candidates.Count == checkPointNum)
                    {
                        break;
                    }
                }
                if (candidates.Count == checkPointNum)
                {
                    break;
                }
                else
                {
                    candidates.Clear();
                }
            }

            for (int i = 0; i < candidates.Count; i++)
            {
                extendMaze[candidates[i].x - 1, candidates[i].y] = MazeConstants.MazeObjKinds.EChecPoint;
                extendMaze[candidates[i].x, candidates[i].y] = MazeConstants.MazeObjKinds.EChecPoint;
                extendMaze[candidates[i].x + 1, candidates[i].y] = MazeConstants.MazeObjKinds.EChecPoint;
            }
        }


        //迷路の拡張
        private void ExtendMaze(MazeConstants.MazeObjKinds[,] orgMaze )
        {
            // 頻繫にアクセスするためラッチする
            int latSize = size;
            int latRate = makeMazeData.rate;

            //道幅などを調整するために壁の厚みはそのままにし、latRate倍したステージにする
            for (int y = 0; y < latSize; y++)
            {
                for (int x = 0; x < latSize; x++)
                {
                    //迷路は第2象限を基準に作成する
                    //    y
                    // 　→
                    // x↓

                    // 外周にあたる箇所なので壁はEUnBreakWallに設定する
                    if (x == 0 || y == 0 || x == latSize - 1 || y == latSize - 1)
                    {
                        //外周は壁にしたままだが、余りを通路にする
                        //2*2となるので角は壁にして内側は通路にする
                        if (x == 0)
                        {
                            if (y == 0)
                            {
                                /*  ○○○○  */
                                /*  ○×××  */
                                /*  ○×××  */
                                /*  ○×××  */
                                for(int loopx = 0; loopx< latRate; loopx++ )
                                {
                                    for (int loopy = 0; loopy < latRate; loopy++)
                                    {
                                        if (loopx ==0 || loopy ==0)
                                        {
                                            this.extendMaze[x * latRate + loopx, y * latRate + loopy] = MazeConstants.MazeObjKinds.EUnBreakWall;
                                        }
                                        else
                                        {
                                            this.extendMaze[x * latRate + 1, y * latRate + 1] = MazeConstants.MazeObjKinds.EPath;
                                        }
                                    }
                                }
                            }
                            else if (y == latSize - 1)
                            {
                                /*  ○○○○  */
                                /*  ×××○  */
                                /*  ×××○  */
                                /*  ×××○  */
                                for (int loopx = 0; loopx < latRate; loopx++)
                                {
                                    for (int loopy = 0; loopy < latRate; loopy++)
                                    {
                                        if (loopx == 0 || loopy == latRate-1)
                                        {
                                            this.extendMaze[x * latRate + loopx, y * latRate + loopy] = MazeConstants.MazeObjKinds.EUnBreakWall;
                                        }
                                        else
                                        {
                                            this.extendMaze[x * latRate + 1, y * latRate + 1] = MazeConstants.MazeObjKinds.EPath;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                /*  ○○○○  */
                                /*  ××××  */
                                /*  ××××  */
                                /*  ××××  */
                                for (int loopx = 0; loopx < latRate; loopx++)
                                {
                                    for (int loopy = 0; loopy < latRate; loopy++)
                                    {
                                        if(loopx ==0)
                                        {
                                            this.extendMaze[x * latRate + loopx, y * latRate + loopy] = MazeConstants.MazeObjKinds.EUnBreakWall;
                                        }
                                        else
                                        {
                                            this.extendMaze[x * latRate + loopx, y * latRate + loopy] = MazeConstants.MazeObjKinds.EPath;
                                        }
                                    }
                                }
                                //外壁と迷路の壁がつながっているとき
                                if (orgMaze[x + 1, y] != MazeConstants.MazeObjKinds.EPath)
                                {
                                    for (int loop=0; loop < latRate; loop++)
                                    {
                                        this.extendMaze[x * latRate + loop, y * latRate + 0] = MazeConstants.MazeObjKinds.EBreakWall;
                                    }
                                }
                            }
                        }
                        else if (x == latSize - 1)
                        {

                            if (y == 0)
                            {
                                /*  ○×××  */
                                /*  ○×××  */
                                /*  ○×××  */
                                /*  ○○○○  */
                                for (int loopx = 0; loopx < latRate; loopx++)
                                {
                                    for (int loopy = 0; loopy < latRate; loopy++)
                                    {
                                        if (loopx == latRate-1 || loopy == 0)
                                        {
                                            this.extendMaze[x * latRate + loopx, y * latRate + loopy] = MazeConstants.MazeObjKinds.EUnBreakWall;
                                        }
                                        else
                                        {
                                            this.extendMaze[x * latRate + 1, y * latRate + 1] = MazeConstants.MazeObjKinds.EPath;
                                        }
                                    }
                                }
                            }
                            else if (y == latSize - 1)
                            {
                                /*  ×××○  */
                                /*  ×××○  */
                                /*  ×××○  */
                                /*  ○○○○  */
                                for (int loopx = 0; loopx < latRate; loopx++)
                                {
                                    for (int loopy = 0; loopy < latRate; loopy++)
                                    {
                                        if (loopx == latRate - 1 || loopy == latRate-1)
                                        {
                                            this.extendMaze[x * latRate + loopx, y * latRate + loopy] = MazeConstants.MazeObjKinds.EUnBreakWall;
                                        }
                                        else
                                        {
                                            this.extendMaze[x * latRate + 1, y * latRate + 1] = MazeConstants.MazeObjKinds.EPath;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                /*  ××××  */
                                /*  ××××  */
                                /*  ××××  */
                                /*  ○○○○  */
                                for (int loopx = 0; loopx < latRate; loopx++)
                                {
                                    for (int loopy = 0; loopy < latRate; loopy++)
                                    {
                                        if (loopx == latRate-1)
                                        {
                                            this.extendMaze[x * latRate + loopx, y * latRate + loopy] = MazeConstants.MazeObjKinds.EUnBreakWall;
                                        }
                                        else
                                        {
                                            this.extendMaze[x * latRate + loopx, y * latRate + loopy] = MazeConstants.MazeObjKinds.EPath;
                                        }
                                    }
                                }
                                //外壁と迷路の壁がつながっているとき
                                if (orgMaze[x-1, y] != MazeConstants.MazeObjKinds.EPath)
                                {
                                    for(int loop = 0;loop < latRate;loop++)
                                    {
                                        this.extendMaze[x * latRate + loop, y * latRate + 0] = MazeConstants.MazeObjKinds.EBreakWall;
                                    }
                                }
                            }
                        }
                        else if (y == 0)
                        {
                            //角はx側で吸収済みなので考慮不要
                            /*  ○×××  */
                            /*  ○×××  */
                            /*  ○×××  */
                            /*  ○×××  */
                            for (int loopx = 0; loopx < latRate; loopx++)
                            {
                                for (int loopy = 0; loopy < latRate; loopy++)
                                {
                                    if (loopy == 0)
                                    {
                                        this.extendMaze[x * latRate + loopx, y * latRate + loopy] = MazeConstants.MazeObjKinds.EUnBreakWall;
                                    }
                                    else
                                    {
                                        this.extendMaze[x * latRate + loopx, y * latRate + loopy] = MazeConstants.MazeObjKinds.EPath;
                                    }
                                }
                            }
                            //外壁と迷路の壁がつながっているとき
                            if (orgMaze[x, y + 1] != MazeConstants.MazeObjKinds.EPath)
                            {
                                for (int loop = 0; loop < latRate; loop++)
                                {
                                    this.extendMaze[x * latRate +0, y * latRate + loop] = MazeConstants.MazeObjKinds.EBreakWall;

                                }
                            }
                        }
                        else /* (y == latSize - 1) */
                        {

                            //角はx側で吸収済みなので考慮不要
                            /*  ×××○  */
                            /*  ×××○  */
                            /*  ×××○  */
                            /*  ×××○  */
                            for (int loopx = 0; loopx < latRate; loopx++)
                            {
                                for (int loopy = 0; loopy < latRate; loopy++)
                                {
                                    if (loopy == latRate-1)
                                    {
                                        this.extendMaze[x * latRate + loopx, y * latRate + loopy] = MazeConstants.MazeObjKinds.EUnBreakWall;
                                    }
                                    else
                                    {
                                        this.extendMaze[x * latRate + loopx, y * latRate + loopy] = MazeConstants.MazeObjKinds.EPath;
                                    }
                                }
                            }
                            //外壁と迷路の壁がつながっているとき
                            if (orgMaze[x, y - 1] != MazeConstants.MazeObjKinds.EPath)
                            {
                                for (int loop = 0; loop < latRate; loop++)
                                {
                                    this.extendMaze[x * latRate + 0, y * latRate + loop] = MazeConstants.MazeObjKinds.EBreakWall;
                                }
                            }
                        }
                    }
                    else
                    {
                        //範囲内は一度通路に設定する
                        for (int loopx = 0; loopx < latRate; loopx++)
                        {
                            for (int loopy = 0; loopy < latRate; loopy++)
                            {
                                this.extendMaze[x * latRate + loopx, y * latRate + loopy] = MazeConstants.MazeObjKinds.EPath;
                            }
                        }
                         //起点となる場所はそのまま
                         this.extendMaze[x * latRate + 0, y * latRate + 0] = orgMaze[x, y];
                         //マップが延びている方向に延ばす
                         if (orgMaze[x + 1, y] != MazeConstants.MazeObjKinds.EPath)
                         {
                            //上側に伸びている場合
                            for (int loop = 0; loop < latRate; loop++)
                            {
                                this.extendMaze[x * latRate + loop, y * latRate + 0] = orgMaze[x, y];

                            }
                        }
                        if (orgMaze[x, y + 1] != MazeConstants.MazeObjKinds.EPath)
                         {
                            //右側に伸びている場合
                            for (int loop = 0; loop < latRate; loop++)
                            {
                                this.extendMaze[x * latRate + 0, y * latRate + loop] = orgMaze[x, y];

                            }
                        }

                    }
                }
            }
        }

        private void SetupMazeObjects()
        {
            int stageSize = GetStageSize();

            // 1. まず配置候補となる床（EPath）と壊せる壁（EBreakWall）の座標をリストアップする
            List<Vector2Int> pathCandidates = new List<Vector2Int>();
            List<Vector2Int> breakWallCandidates = new List<Vector2Int>();

            for (int y = 1; y < stageSize - 1; y++)
            {
                for (int x = 1; x < stageSize - 1; x++)
                {
                    if (extendMaze[x, y] == MazeConstants.MazeObjKinds.EPath)
                    {
                        pathCandidates.Add(new Vector2Int(x, y));
                    }
                    else if (extendMaze[x, y] == MazeConstants.MazeObjKinds.EBreakWall)
                    {
                        breakWallCandidates.Add(new Vector2Int(x, y));
                    }
                }
            }

            // 候補リストをあらかじめシャッフルしておく（これで先頭から取るだけでランダムになる）
            pathCandidates.Shuffle();
            breakWallCandidates.Shuffle();

            // 十分に離れている距離の閾値（ステージサイズに応じて調整）
            float minDistance = stageSize * 0.6f;


            // 罠（Trap）の配置
            int placedTraps = 0;
            List<Vector2Int> placedObjectPositions = new List<Vector2Int>();

            // オブジェクト同士を離す閾値
            float objectSpacing = makeMazeData.rate * 2f;

            // 床に仕掛ける罠
            for (int i = pathCandidates.Count - 1; i >= 0 && placedTraps < makeMazeData.trapNum; i--)
            {
                Vector2Int pos = pathCandidates[i];
                if (IsFarEnough(pos, placedObjectPositions, objectSpacing))
                {
                    extendMaze[pos.x, pos.y] = MazeConstants.MazeObjKinds.ETrapPath;
                    placedObjectPositions.Add(pos);
                    pathCandidates.RemoveAt(i);
                    placedTraps++;
                }
            }
            // 壁に仕掛ける罠（足りない分）
            for (int i = breakWallCandidates.Count - 1; i >= 0 && placedTraps < makeMazeData.trapNum; i--)
            {
                Vector2Int pos = breakWallCandidates[i];
                if (IsFarEnough(pos, placedObjectPositions, objectSpacing))
                {
                    extendMaze[pos.x, pos.y] = MazeConstants.MazeObjKinds.ETrapWall;
                    placedObjectPositions.Add(pos);
                    breakWallCandidates.RemoveAt(i);
                    placedTraps++;
                }
            }

            // 敵（Enemy）の配置
            int placedEnemies = 0;
            for (int i = pathCandidates.Count - 1; i >= 0 && placedEnemies < makeMazeData.enemyNum; i--)
            {
                Vector2Int pos = pathCandidates[i];
                if (IsFarEnough(pos, placedObjectPositions, objectSpacing))
                {
                    extendMaze[pos.x, pos.y] = MazeConstants.MazeObjKinds.EEnemyPos;
                    placedObjectPositions.Add(pos);
                    pathCandidates.RemoveAt(i);
                    placedEnemies++;
                }
            }

            // アイテム（Item）の配置
            int placedItems = 0;
            for (int i = pathCandidates.Count - 1; i >= 0 && placedItems < makeMazeData.itemNum; i--)
            {
                Vector2Int pos = pathCandidates[i];
                if (IsFarEnough(pos, placedObjectPositions, objectSpacing))
                {
                    extendMaze[pos.x, pos.y] = MazeConstants.MazeObjKinds.EItem;
                    placedObjectPositions.Add(pos);
                    pathCandidates.RemoveAt(i);
                    placedItems++;
                }
            }
        }

        // 他のオブジェクトから十分に離れているかチェックするヘルパー関数
        private bool IsFarEnough(Vector2Int target, List<Vector2Int> others, float minDst)
        {
            // 比較用の閾値をあらかじめ2乗しておく
            float minDstSqr = minDst * minDst;

            foreach (var other in others)
            {
                Vector2Int distVec2 = other - target;

                float sqrLen = distVec2.sqrMagnitude;

                if (sqrLen < minDstSqr)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
