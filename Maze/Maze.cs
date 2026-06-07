using UnityEngine;
using System.Collections.Generic;
using static MazeGame.MazeGameConstants.MazeConstants;

namespace MazeGame
{
    public class Maze : Singleton<Maze>
    {
        public enum CheckSize
        {
            Around,
            Rate
        };
        private MazeExtends maze;
        private MazeObjKinds[,] mazeData;
        private int rate;
        public Transform startTransform { get; set; }
        public Transform goalTransform { get; set; }
        public List<Transform> checkTransform { get; set; }
        public GameObject[,] stageObjects { get; set; }


        public void MakeMaze(int _size, int _trapNum, int _enemyNum,int _rate,int _checkPointNum,int _itemNum)
        {
            maze = new MazeExtends();
            maze.SetMazeData(_size, _trapNum, _enemyNum,_rate, _checkPointNum, _itemNum);
            mazeData = maze.GetMazeData();
            rate = _rate;
            startTransform = null;
            goalTransform = null;
            stageObjects = new GameObject[GetStageSize(), GetStageSize()];
            checkTransform = new List<Transform>();

        }

        public int GetRate()
        { 
            return rate; 
        }

        //ステージサイズ取得
        public int GetStageSize()
        {
            return maze.GetStageSize();
        }

        //迷路情報取得
        public MazeObjKinds[,] GetMazeData()
        {
            return maze.GetMazeData();
        }
        public void GetObjectPos(GameObject target, ref int column, ref int row)
        {
            for (int cloop = 0; cloop < GetStageSize(); cloop++)
            {
                for (int rloop = 0; rloop < GetStageSize(); rloop++)
                {
                    if (target == stageObjects[cloop, rloop])
                    {
                        column = cloop;
                        row = rloop;
                        return;
                    }

                }
            }
        }

        public Vector3 GetObjectPos(int column, int row)
        {
            return stageObjects[column, row].transform.position;
        }

        public MazeObjKinds GetStageinfo(int column, int row)
        {
            return mazeData[column, row];
        }


        private bool CheckWall(Direct direct, int column, int row ,int checkAround)
        {
            int x = 0;
            int y = 0;
            int loopCnt = 0;
            switch (direct)
            {
                case Direct.North:
                    x = 0;
                    y = 1;
                    break;
                case Direct.South:
                    x = 0;
                    y = -1;
                    break;
                case Direct.West:
                    x = -1;
                    y = 0;
                    break;
                case Direct.East:
                    x = 1;
                    y = 0;
                    break;
                default:
                    return false;
            }

            bool wallFlag = false;
            do
            {
                column += x;
                row += y;
                wallFlag = wallFlag || JudegeWall(column, row);
                loopCnt++;

            // 壁にぶつかっていない or 拡張率だけ判定していない or 判定が場外になっていない
            } while (loopCnt < checkAround);

            // true:壁である
            // false:壁でない
            return wallFlag;
        }

        public bool JudegeWall(int column, int row)
        {
            if (column < 0 || GetStageSize() <= column) return false;
            if (row < 0 || GetStageSize() <= row) return false;
            bool breakWall = mazeData[column, row] == MazeObjKinds.EBreakWall;
            bool trapWall = mazeData[column, row] == MazeObjKinds.ETrapWall;
            bool unbreakWall = mazeData[column, row] == MazeObjKinds.EUnBreakWall;

            if(breakWall || trapWall || unbreakWall)
            {
                // 壁である
                return true;
            }
            else
            {
                //壁でない
                return false;
            }
        }

        // 曲がり角
        public bool JudgeCorner(int column, int row, CheckSize size)
        {
            bool rightFlag;
            bool leftFlag;
            bool backFlag;
            bool frontFlag;
            if (size == CheckSize.Around)
            {
                rightFlag = CheckWall(Direct.East, column, row, 1);
                leftFlag = CheckWall(Direct.West, column, row, 1);
                backFlag = CheckWall(Direct.South, column, row, 1);
                frontFlag = CheckWall(Direct.North, column, row, 1);
            }
            else
            {
                rightFlag = CheckWall(Direct.East, column, row, rate);
                leftFlag = CheckWall(Direct.West, column, row, rate);
                backFlag = CheckWall(Direct.South, column, row, rate);
                frontFlag = CheckWall(Direct.North, column, row, rate);
            }
            if ((backFlag && !frontFlag && rightFlag && !leftFlag) || // ┘
                (backFlag && !frontFlag && !rightFlag && leftFlag) || // └
                (!backFlag && frontFlag && rightFlag && !leftFlag) || // ┐
                (!backFlag && frontFlag && !rightFlag && leftFlag))   // ┌
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        // T字路
        public bool JudgeTJunction(int column, int row, CheckSize size)
        {
            bool rightFlag;
            bool leftFlag;
            bool backFlag;
            bool frontFlag;
            if (size == CheckSize.Around)
            {
                rightFlag = CheckWall(Direct.East, column, row, 1);
                leftFlag = CheckWall(Direct.West, column, row, 1);
                backFlag = CheckWall(Direct.South, column, row, 1);
                frontFlag = CheckWall(Direct.North, column, row, 1);
            }
            else
            {
                rightFlag = CheckWall(Direct.East, column, row, rate);
                leftFlag = CheckWall(Direct.West, column, row, rate);
                backFlag = CheckWall(Direct.South, column, row, rate);
                frontFlag = CheckWall(Direct.North, column, row, rate);
            }
            if ((!backFlag && frontFlag && rightFlag && leftFlag) || // ┴
                (backFlag && !frontFlag && rightFlag && leftFlag) || // ┳
                (backFlag && frontFlag && !rightFlag && leftFlag) || // ┤
                (backFlag && frontFlag && rightFlag && !leftFlag))   // ┠
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // 行き止まり
        public bool JudgeDeadEnd(int column, int row, CheckSize size)
        {
            bool rightFlag;
            bool leftFlag;
            bool backFlag;
            bool frontFlag;
            if (size == CheckSize.Around)
            {
                rightFlag = CheckWall(Direct.East, column, row, 1);
                leftFlag = CheckWall(Direct.West, column, row, 1);
                backFlag = CheckWall(Direct.South, column, row, 1);
                frontFlag = CheckWall(Direct.North, column, row, 1);
            }
            else
            {
                rightFlag = CheckWall(Direct.East, column, row, rate);
                leftFlag = CheckWall(Direct.West, column, row, rate);
                backFlag = CheckWall(Direct.South, column, row, rate);
                frontFlag = CheckWall(Direct.North, column, row, rate);
            }
            if ((backFlag && frontFlag && rightFlag && !leftFlag) || // 左行き止まり
                (backFlag && frontFlag && !rightFlag && leftFlag) || // 右行き止まり
                (backFlag && !frontFlag && rightFlag && leftFlag) || // 前方行き止まり
                (!backFlag && frontFlag && rightFlag && leftFlag))   // 後方行き止まり
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // 前後左右行き止まり
        public bool JudgeSiege(int column, int row, CheckSize size)
        {
            bool rightFlag;
            bool leftFlag;
            bool backFlag;
            bool frontFlag;
            if (size == CheckSize.Around)
            {
                rightFlag = CheckWall(Direct.East, column, row, 1);
                leftFlag = CheckWall(Direct.West, column, row, 1);
                backFlag = CheckWall(Direct.South, column, row, 1);
                frontFlag = CheckWall(Direct.North, column, row, 1);
            }
            else
            {
                rightFlag = CheckWall(Direct.East, column, row, rate);
                leftFlag = CheckWall(Direct.West, column, row, rate);
                backFlag = CheckWall(Direct.South, column, row, rate);
                frontFlag = CheckWall(Direct.North, column, row, rate);
            }

            if (rightFlag && leftFlag && backFlag && frontFlag)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Direct JudeAround(int column, int row)
        {
            bool rightFlag = CheckWall(Direct.East, column, row, 1);
            bool leftFlag = CheckWall(Direct.West, column, row, 1);
            bool backFlag = CheckWall(Direct.South, column, row, 1);
            bool frontFlag = CheckWall(Direct.North, column, row, 1);
            if(rightFlag && leftFlag && backFlag && frontFlag)
            {
                return Direct.Siege;
            }
            else if (!rightFlag && !leftFlag && !backFlag && !frontFlag)
            {
                return Direct.Nothing;
            }
            else if (!frontFlag)
            {
                return Direct.North;
            }
            else if (!backFlag)
            {
                return Direct.South;
            }
            else if (!rightFlag )
            {
                return Direct.West;
            }
            else if (!leftFlag)
            {
                return Direct.East;
            }
            else
            {
                return Direct.Nothing;
            }
        }

        // 方向判定
        public Direct JudgeDirect(int column, int row, CheckSize size)
        {
            bool rightFlag;
            bool leftFlag;
            bool backFlag;
            bool frontFlag;
            if (size == CheckSize.Around)
            {
                rightFlag = CheckWall(Direct.East, column, row, 1);
                leftFlag = CheckWall(Direct.West, column, row, 1);
                backFlag = CheckWall(Direct.South, column, row, 1);
                frontFlag = CheckWall(Direct.North, column, row, 1);
            }
            else
            {
                rightFlag = CheckWall(Direct.East, column, row, rate);
                leftFlag = CheckWall(Direct.West, column, row, rate);
                backFlag = CheckWall(Direct.South, column, row, rate);
                frontFlag = CheckWall(Direct.North, column, row, rate);
            }
            if (!backFlag)
            {
                return Direct.South;
            }
            else if (!frontFlag)
            {
                return Direct.North;
            }
            else if (!rightFlag)
            {
                return Direct.East;
            }
            else if (!leftFlag)
            {
                return Direct.West;
            }
            else
            {
                //基本的にここには到達しないのでNothingを返す
                return Direct.Nothing;
            }

        }

    }
}
