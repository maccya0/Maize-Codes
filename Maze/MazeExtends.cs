using System.Collections.Generic;
using UnityEngine;
using static MazeGame.MazeGameConstants.MazeConstants;

namespace MazeGame
{
    public class MazeExtends : DiggingHoleMaze
    {
        private int rate; // 迷路拡張倍率
        private int trapNum;    // 罠の生成数
        private int enemyNum;    // 敵の生成数
        private int itemNum;
        private MazeObjKinds[,] extendMaze; // 拡張した迷路

        //迷路生成データセット
        public void SetMazeData(int _size, int _tarapNum, int _enemyNum, int _rate, int _checkPointNum, int _itemNum)
        {
            MazeObjKinds[,] tempExtendMaze;
            base.SetMazeData(_size);
            tempExtendMaze = base.CreateMaze();

            this.trapNum = _tarapNum;
            this.enemyNum = _enemyNum;
            this.rate = _rate;
            this.extendMaze = new MazeObjKinds[base.size * rate, base.size * rate];
            itemNum = _itemNum;

            ExtendMaze(tempExtendMaze);
            SetGoalPos();
            SetStartPos();
            SetCheckPoint(_checkPointNum);

            // 修正：スタート・ゴールの確定とオブジェクト配置を安全に行う
            SetupMazeObjects();
        }

        public int GetStageSize()
        {
            return base.size * rate;
        }

        public MazeObjKinds[,] GetMazeData()
        {
            return extendMaze;
        }
        private void SetGoalPos()
        {
            int stageSize = GetStageSize();
            for (int x = stageSize - 2; x >1; x--)
            {
                if ( (extendMaze[stageSize - 1, x-1] == MazeObjKinds.EUnBreakWall) &&
                     (extendMaze[stageSize - 2, x-1] == MazeObjKinds.EPath)&&
                     (extendMaze[stageSize - 1, x] == MazeObjKinds.EUnBreakWall) &&
                     (extendMaze[stageSize - 2, x] == MazeObjKinds.EPath)&&
                     (extendMaze[stageSize - 1, x+1] == MazeObjKinds.EUnBreakWall) &&
                     (extendMaze[stageSize - 2, x+1] == MazeObjKinds.EPath))
                {
                    extendMaze[stageSize - 1, x-1] = MazeObjKinds.EGoal;
                    extendMaze[stageSize - 1, x] = MazeObjKinds.EGoal;
                    extendMaze[stageSize - 1, x+1] = MazeObjKinds.EGoal;
                    break;
                }
            }
        }
        private void SetStartPos()
        {
            for (int x = 2; x < GetStageSize() - 2; x++)
            {
                if ((extendMaze[0, x - 1] == MazeObjKinds.EUnBreakWall) &&
                     (extendMaze[1, x - 1] == MazeObjKinds.EPath) &&
                     (extendMaze[0, x ] == MazeObjKinds.EUnBreakWall) &&
                     (extendMaze[1, x ] == MazeObjKinds.EPath) &&
                     (extendMaze[0, x + 1] == MazeObjKinds.EUnBreakWall) &&
                     (extendMaze[1, x + 1] == MazeObjKinds.EPath))
                {
                    extendMaze[0, x-1] = MazeObjKinds.EStart;
                    extendMaze[0, x] = MazeObjKinds.EStart;
                    extendMaze[0, x+1] = MazeObjKinds.EStart;
                    break;
                }
            }
        }
        private void SetCheckPoint(int checkPointNum)
        {

            int checkNum = 0;
            int stageSize = GetStageSize();
            List<int> posList = new List<int>();
            do
            {
                int pos = Random.Range(rate*2, stageSize - rate);
                bool nextLoop=false;
                for(int checkLoop = 0; checkLoop< posList.Count;checkLoop++)
                {
                    if(rate * 3 > Mathf.Abs(pos- posList[checkLoop]))
                    {
                        nextLoop = true;
                    }
                }
                if (nextLoop) continue;
                bool randomBool = Random.value > 0.5f;
                if (randomBool)
                {
                    if ((extendMaze[pos - 1, 0] == MazeObjKinds.EUnBreakWall) &&
                        (extendMaze[pos - 1, 1] == MazeObjKinds.EPath) &&
                        (extendMaze[pos, 0] == MazeObjKinds.EUnBreakWall) &&
                        (extendMaze[pos, 1] == MazeObjKinds.EPath) &&
                        (extendMaze[pos + 1, 0] == MazeObjKinds.EUnBreakWall) &&
                        (extendMaze[pos + 1, 1] == MazeObjKinds.EPath))
                    {
                        extendMaze[pos - 1, 0] = MazeObjKinds.EChecPoint;
                        extendMaze[pos, 0] = MazeObjKinds.EChecPoint;
                        extendMaze[pos + 1, 0] = MazeObjKinds.EChecPoint;
                        checkNum++;
                        posList.Add(pos);
                    }
                }
                else
                {
                    if ((extendMaze[pos - 1, stageSize - 1] == MazeObjKinds.EUnBreakWall) &&
                        (extendMaze[pos - 1, stageSize - 2] == MazeObjKinds.EPath) &&
                        (extendMaze[pos, stageSize - 1] == MazeObjKinds.EUnBreakWall) &&
                        (extendMaze[pos, stageSize - 2] == MazeObjKinds.EPath) &&
                        (extendMaze[pos + 1, stageSize - 1] == MazeObjKinds.EUnBreakWall) &&
                        (extendMaze[pos + 1, stageSize - 2] == MazeObjKinds.EPath))
                    {
                        extendMaze[pos - 1, stageSize - 1] = MazeObjKinds.EChecPoint;
                        extendMaze[pos, stageSize - 1] = MazeObjKinds.EChecPoint;
                        extendMaze[pos + 1, stageSize - 1] = MazeObjKinds.EChecPoint;
                        checkNum++;
                        posList.Add(pos);
                    }
                }
            } while (checkNum != checkPointNum);
        }

        //迷路の拡張
        private void ExtendMaze(MazeObjKinds[,] orgMaze )
        {
            //道幅などを調整するために壁の厚みはそのままにし、rate倍したステージにする
            for (int y = 0; y < base.size; y++)
            {
                for (int x = 0; x < base.size; x++)
                {
                    //迷路は第2象限を基準に作成する
                    //    y
                    // 　→
                    // x↓

                    // 外周にあたる箇所なので壁はEUnBreakWallに設定する
                    if (x == 0 || y == 0 || x == base.size - 1 || y == base.size - 1)
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
                                for(int loopx = 0; loopx< rate; loopx++ )
                                {
                                    for (int loopy = 0; loopy < rate; loopy++)
                                    {
                                        if (loopx ==0 || loopy ==0)
                                        {
                                            this.extendMaze[x * rate + loopx, y * rate + loopy] = MazeObjKinds.EUnBreakWall;
                                        }
                                        else
                                        {
                                            this.extendMaze[x * rate + 1, y * rate + 1] = MazeObjKinds.EPath;
                                        }
                                    }
                                }
                            }
                            else if (y == base.size - 1)
                            {
                                /*  ○○○○  */
                                /*  ×××○  */
                                /*  ×××○  */
                                /*  ×××○  */
                                for (int loopx = 0; loopx < rate; loopx++)
                                {
                                    for (int loopy = 0; loopy < rate; loopy++)
                                    {
                                        if (loopx == 0 || loopy == rate-1)
                                        {
                                            this.extendMaze[x * rate + loopx, y * rate + loopy] = MazeObjKinds.EUnBreakWall;
                                        }
                                        else
                                        {
                                            this.extendMaze[x * rate + 1, y * rate + 1] = MazeObjKinds.EPath;
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
                                for (int loopx = 0; loopx < rate; loopx++)
                                {
                                    for (int loopy = 0; loopy < rate; loopy++)
                                    {
                                        if(loopx ==0)
                                        {
                                            this.extendMaze[x * rate + loopx, y * rate + loopy] = MazeObjKinds.EUnBreakWall;
                                        }
                                        else
                                        {
                                            this.extendMaze[x * rate + loopx, y * rate + loopy] = MazeObjKinds.EPath;
                                        }
                                    }
                                }
                                //外壁と迷路の壁がつながっているとき
                                if (orgMaze[x + 1, y] != MazeObjKinds.EPath)
                                {
                                    for (int loop=0; loop < rate; loop++)
                                    {
                                        this.extendMaze[x * rate + loop, y * rate + 0] = MazeObjKinds.EBreakWall;
                                    }
                                }
                            }
                        }
                        else if (x == base.size - 1)
                        {

                            if (y == 0)
                            {
                                /*  ○×××  */
                                /*  ○×××  */
                                /*  ○×××  */
                                /*  ○○○○  */
                                for (int loopx = 0; loopx < rate; loopx++)
                                {
                                    for (int loopy = 0; loopy < rate; loopy++)
                                    {
                                        if (loopx == rate-1 || loopy == 0)
                                        {
                                            this.extendMaze[x * rate + loopx, y * rate + loopy] = MazeObjKinds.EUnBreakWall;
                                        }
                                        else
                                        {
                                            this.extendMaze[x * rate + 1, y * rate + 1] = MazeObjKinds.EPath;
                                        }
                                    }
                                }
                            }
                            else if (y == base.size - 1)
                            {
                                /*  ×××○  */
                                /*  ×××○  */
                                /*  ×××○  */
                                /*  ○○○○  */
                                for (int loopx = 0; loopx < rate; loopx++)
                                {
                                    for (int loopy = 0; loopy < rate; loopy++)
                                    {
                                        if (loopx == rate - 1 || loopy == rate-1)
                                        {
                                            this.extendMaze[x * rate + loopx, y * rate + loopy] = MazeObjKinds.EUnBreakWall;
                                        }
                                        else
                                        {
                                            this.extendMaze[x * rate + 1, y * rate + 1] = MazeObjKinds.EPath;
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
                                for (int loopx = 0; loopx < rate; loopx++)
                                {
                                    for (int loopy = 0; loopy < rate; loopy++)
                                    {
                                        if (loopx == rate-1)
                                        {
                                            this.extendMaze[x * rate + loopx, y * rate + loopy] = MazeObjKinds.EUnBreakWall;
                                        }
                                        else
                                        {
                                            this.extendMaze[x * rate + loopx, y * rate + loopy] = MazeObjKinds.EPath;
                                        }
                                    }
                                }
                                //外壁と迷路の壁がつながっているとき
                                if (orgMaze[x-1, y] != MazeObjKinds.EPath)
                                {
                                    for(int loop = 0;loop < rate;loop++)
                                    {
                                        this.extendMaze[x * rate + loop, y * rate + 0] = MazeObjKinds.EBreakWall;
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
                            for (int loopx = 0; loopx < rate; loopx++)
                            {
                                for (int loopy = 0; loopy < rate; loopy++)
                                {
                                    if (loopy == 0)
                                    {
                                        this.extendMaze[x * rate + loopx, y * rate + loopy] = MazeObjKinds.EUnBreakWall;
                                    }
                                    else
                                    {
                                        this.extendMaze[x * rate + loopx, y * rate + loopy] = MazeObjKinds.EPath;
                                    }
                                }
                            }
                            //外壁と迷路の壁がつながっているとき
                            if (orgMaze[x, y + 1] != MazeObjKinds.EPath)
                            {
                                for (int loop = 0; loop < rate; loop++)
                                {
                                    this.extendMaze[x * rate +0, y * rate + loop] = MazeObjKinds.EBreakWall;

                                }
                            }
                        }
                        else /* (y == base.size - 1) */
                        {

                            //角はx側で吸収済みなので考慮不要
                            /*  ×××○  */
                            /*  ×××○  */
                            /*  ×××○  */
                            /*  ×××○  */
                            for (int loopx = 0; loopx < rate; loopx++)
                            {
                                for (int loopy = 0; loopy < rate; loopy++)
                                {
                                    if (loopy == rate-1)
                                    {
                                        this.extendMaze[x * rate + loopx, y * rate + loopy] = MazeObjKinds.EUnBreakWall;
                                    }
                                    else
                                    {
                                        this.extendMaze[x * rate + loopx, y * rate + loopy] = MazeObjKinds.EPath;
                                    }
                                }
                            }
                            //外壁と迷路の壁がつながっているとき
                            if (orgMaze[x, y - 1] != MazeObjKinds.EPath)
                            {
                                for (int loop = 0; loop < rate; loop++)
                                {
                                    this.extendMaze[x * rate + 0, y * rate + loop] = MazeObjKinds.EBreakWall;
                                }
                            }
                        }
                    }
                    else
                    {
                        //範囲内は一度通路に設定する
                        for (int loopx = 0; loopx < 4; loopx++)
                        {
                            for (int loopy = 0; loopy < 4; loopy++)
                            {
                                this.extendMaze[x * rate + loopx, y * rate + loopy] = MazeObjKinds.EPath;
                            }
                        }
                         //起点となる場所はそのまま
                         this.extendMaze[x * rate + 0, y * rate + 0] = orgMaze[x, y];
                         //マップが延びている方向に延ばす
                         if (orgMaze[x + 1, y] != MazeObjKinds.EPath)
                         {
                            //上側に伸びている場合
                            for (int loop = 0; loop < rate; loop++)
                            {
                                this.extendMaze[x * rate + loop, y * rate + 0] = orgMaze[x, y];

                            }
                        }
                        if (orgMaze[x, y + 1] != MazeObjKinds.EPath)
                         {
                            //右側に伸びている場合
                            for (int loop = 0; loop < rate; loop++)
                            {
                                this.extendMaze[x * rate + 0, y * rate + loop] = orgMaze[x, y];

                            }
                        }

                    }
                }
            }
        }

        private bool JudegeSetPos(int x ,int y)
        {

            if ((extendMaze[x - 1, y] != MazeObjKinds.EPath) ||
                (extendMaze[x + 1, y] != MazeObjKinds.EPath) ||
                (extendMaze[x , y - 1] != MazeObjKinds.EPath) ||
                (extendMaze[x , y + 1] != MazeObjKinds.EPath) )
            {
                return true;
            }
            else
            {
                return false;
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
                    if (extendMaze[x, y] == MazeObjKinds.EPath)
                    {
                        pathCandidates.Add(new Vector2Int(x, y));
                    }
                    else if (extendMaze[x, y] == MazeObjKinds.EBreakWall)
                    {
                        breakWallCandidates.Add(new Vector2Int(x, y));
                    }
                }
            }

            // シャッフル（ランダムに並び替え）用の共通関数
            void ShuffleList<T>(List<T> list)
            {
                for (int i = list.Count - 1; i > 0; i--)
                {
                    int r = Random.Range(0, i + 1);
                    T tmp = list[i];
                    list[i] = list[r];
                    list[r] = tmp;
                }
            }

            // 候補リストをあらかじめシャッフルしておく（これで先頭から取るだけでランダムになる）
            ShuffleList(pathCandidates);
            ShuffleList(breakWallCandidates);

            // 2. スタートとゴールの配置（お互いに離れた位置になる候補をリストから探す）
            Vector2Int startPos = Vector2Int.zero;
            Vector2Int goalPos = Vector2Int.zero;
            bool pairFound = false;

            // 十分に離れている距離の閾値（ステージサイズに応じて調整）
            float minDistance = stageSize * 0.6f;

            // シャッフル済みリストから、条件に合うペアを1回だけ探索（総当たりでも一瞬です）
            for (int i = 0; i < pathCandidates.Count && !pairFound; i++)
            {
                for (int j = i + 1; j < pathCandidates.Count; j++)
                {
                    if (Vector2.Distance(pathCandidates[i], pathCandidates[j]) >= minDistance)
                    {
                        startPos = pathCandidates[i];
                        goalPos = pathCandidates[j];
                        pairFound = true;

                        // リストから除外するために、インデックスの大きい方から削除
                        pathCandidates.RemoveAt(j);
                        pathCandidates.RemoveAt(i);
                        break;
                    }
                }
            }

            // 万が一、離れた位置が見つからなかった場合のセーフティ
            if (!pairFound && pathCandidates.Count >= 2)
            {
                startPos = pathCandidates[0];
                goalPos = pathCandidates[1];
                pathCandidates.RemoveRange(0, 2);
            }

            // マップに適用
            extendMaze[startPos.x, startPos.y] = MazeObjKinds.EStart;
            extendMaze[goalPos.x, goalPos.y] = MazeObjKinds.EGoal;

            // 3. スタート・ゴール周辺（安全圏）の座標を候補リストから除外する
            // これにより、あとから罠を消す必要がなくなり、数が減るバグも防げます
            float safeRadius = base.size / 2f;
            pathCandidates.RemoveAll(pos => Vector2.Distance(pos, startPos) < safeRadius || Vector2.Distance(pos, goalPos) < safeRadius);
            breakWallCandidates.RemoveAll(pos => Vector2.Distance(pos, startPos) < safeRadius || Vector2.Distance(pos, goalPos) < safeRadius);

            // 4. 罠（Trap）の配置
            // ※「オブジェクト同士を離す」という条件は、あらかじめ絞られたリストに対して行う
            int placedTraps = 0;
            List<Vector2Int> placedObjectPositions = new List<Vector2Int>();

            // オブジェクト同士を離す閾値（制限が厳しすぎると配置しきれないので、base.sizeやrateに合わせて調整してください）
            float objectSpacing = rate * 2f;

            // 床に仕掛ける罠
            for (int i = pathCandidates.Count - 1; i >= 0 && placedTraps < trapNum; i--)
            {
                Vector2Int pos = pathCandidates[i];
                if (IsFarEnough(pos, placedObjectPositions, objectSpacing))
                {
                    extendMaze[pos.x, pos.y] = MazeObjKinds.ETrapPath;
                    placedObjectPositions.Add(pos);
                    pathCandidates.RemoveAt(i);
                    placedTraps++;
                }
            }
            // 壁に仕掛ける罠（足りない分）
            for (int i = breakWallCandidates.Count - 1; i >= 0 && placedTraps < trapNum; i--)
            {
                Vector2Int pos = breakWallCandidates[i];
                if (IsFarEnough(pos, placedObjectPositions, objectSpacing))
                {
                    extendMaze[pos.x, pos.y] = MazeObjKinds.ETrapWall;
                    placedObjectPositions.Add(pos);
                    breakWallCandidates.RemoveAt(i);
                    placedTraps++;
                }
            }

            // 5. 敵（Enemy）の配置
            int placedEnemies = 0;
            for (int i = pathCandidates.Count - 1; i >= 0 && placedEnemies < enemyNum; i--)
            {
                Vector2Int pos = pathCandidates[i];
                if (IsFarEnough(pos, placedObjectPositions, objectSpacing))
                {
                    extendMaze[pos.x, pos.y] = MazeObjKinds.EEnemyPos;
                    placedObjectPositions.Add(pos);
                    pathCandidates.RemoveAt(i);
                    placedEnemies++;
                }
            }

            // 6. アイテム（Item）の配置
            int placedItems = 0;
            for (int i = pathCandidates.Count - 1; i >= 0 && placedItems < itemNum; i--)
            {
                Vector2Int pos = pathCandidates[i];
                if (IsFarEnough(pos, placedObjectPositions, objectSpacing))
                {
                    extendMaze[pos.x, pos.y] = MazeObjKinds.EItem;
                    placedObjectPositions.Add(pos);
                    pathCandidates.RemoveAt(i);
                    placedItems++;
                }
            }
        }

        // 他のオブジェクトから十分に離れているかチェックするヘルパー関数
        private bool IsFarEnough(Vector2Int target, List<Vector2Int> others, float minDst)
        {
            foreach (var other in others)
            {
                if (Vector2.Distance(target, other) < minDst)
                {
                    return false; // 近すぎる
                }
            }
            return true; // 離れているのでOK
        }
    }
}
