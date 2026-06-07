using System.Collections.Generic;
using static MazeGame.MazeGameConstants.MazeConstants;

namespace MazeGame
{
    public class DiggingHoleMaze
    {
        protected enum Direction // 方角
        {
            EUp,
            ERight,
            EDown,
            ELeft
        }
        protected struct Cell // 座標
        {
            public int X { get; set; }
            public int Y { get; set; }
            public Cell(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        protected int size;   // ステージの生成元サイズ
        protected System.Random random;   // 乱数
        private MazeObjKinds[,] maze;   // 迷路
        private Stack<Cell> CurrentWallCells;  // 拡張中の壁情報
        private List<Cell> StartCells;  // 開始セルの情報

        // 迷路生成データセット
        public virtual void SetMazeData(int _size)
        {
            // 奇数でないと生成できないので調整
            if (_size % 2 == 0)
            {
                _size++;
            }
            // ステージサイズはその後にCreateMazeする前提
            this.size = _size;
            this.maze = new MazeObjKinds[this.size, this.size];
            this.StartCells = new List<Cell>();
            this.CurrentWallCells = new Stack<Cell>();
            this.random = new System.Random();
        }
        protected MazeObjKinds[,] CreateMaze()
        {
            //  各マスの初期設定を行う
            for (int y = 0; y < this.size; y++)
            {
                for (int x = 0; x < this.size; x++)
                {
                    if (x == 0 || y == 0 || x == this.size - 1 || y == this.size - 1)
                    {
                        //  外周は破壊不可の壁にする
                        this.maze[x, y] = MazeObjKinds.EUnBreakWall;
                    }
                    else
                    {
                        // 一度全部通常の通路に設定する
                        this.maze[x, y] = MazeObjKinds.EPath;
                        //  外周ではない偶数座標を壁伸ばし開始点にしておく
                        if (x % 2 == 0 && y % 2 == 0)
                        {
                            //  開始候補座標
                            StartCells.Add(new Cell(x, y));
                        }
                    }
                }
            }
            //  壁が拡張できなくなるまでループ
            while (StartCells.Count > 0)
            {
                //  ランダムに開始セルを取得し、開始候補から削除
                int index = random.Next(StartCells.Count);
                Cell cell = StartCells[index];
                StartCells.RemoveAt(index);
                int x = cell.X;
                int y = cell.Y;

                //  すでに壁の場合は何もしない
                if (this.maze[x, y] == MazeObjKinds.EPath)
                {
                    //  拡張中の壁情報を初期化
                    CurrentWallCells.Clear();
                    ExtendWall(x, y);
                }
            }
            return this.maze;
        }

        //  指定座標から壁を生成拡張する
        private void ExtendWall(int x, int y)
        {
            //  伸ばすことができる方向(1マス先が通路で2マス先まで範囲内)
            //  2マス先が壁で自分自身の場合、伸ばせない
            var directions = new List<Direction>();
            if (this.maze[x, y - 1] == MazeObjKinds.EPath && !IsCurrentWall(x, y - 2))
                directions.Add(Direction.EUp);
            if (this.maze[x + 1, y] == MazeObjKinds.EPath && !IsCurrentWall(x + 2, y))
                directions.Add(Direction.ERight);
            if (this.maze[x, y + 1] == MazeObjKinds.EPath && !IsCurrentWall(x, y + 2))
                directions.Add(Direction.EDown);
            if (this.maze[x - 1, y] == MazeObjKinds.EPath && !IsCurrentWall(x - 2, y))
                directions.Add(Direction.ELeft);

            //  ランダムに伸ばす(2マス)
            if (directions.Count > 0)
            {
                //  壁を作成(この地点から壁を伸ばす)
                SetWall(x, y);

                //  伸ばす先が通路の場合は拡張を続ける
                var isPath = false;
                var dirIndex = random.Next(directions.Count);
                switch (directions[dirIndex])
                {
                    case Direction.EUp:
                        isPath = (this.maze[x, y - 2] == MazeObjKinds.EPath);
                        SetWall(x, --y);
                        SetWall(x, --y);
                        break;
                    case Direction.ERight:
                        isPath = (this.maze[x + 2, y] == MazeObjKinds.EPath);
                        SetWall(++x, y);
                        SetWall(++x, y);
                        break;
                    case Direction.EDown:
                        isPath = (this.maze[x, y + 2] == MazeObjKinds.EPath);
                        SetWall(x, ++y);
                        SetWall(x, ++y);
                        break;
                    case Direction.ELeft:
                        isPath = (this.maze[x - 2, y] == MazeObjKinds.EPath);
                        SetWall(--x, y);
                        SetWall(--x, y);
                        break;
                }
                if (isPath)
                {
                    //  既存の壁に接続できていない場合は拡張続行
                    ExtendWall(x, y);
                }
            }
            else
            {
                //  すべて現在拡張中の壁にぶつかる場合、バックして再開
                var beforeCell = CurrentWallCells.Pop();
                ExtendWall(beforeCell.X, beforeCell.Y);
            }
        }

        //  壁を拡張する
        private void SetWall(int x, int y)
        {
            this.maze[x, y] = MazeObjKinds.EBreakWall;
            if (x % 2 == 0 && y % 2 == 0)
            {
                CurrentWallCells.Push(new Cell(x, y));
            }
        }

        //  拡張中の座標かどうか判定
        private bool IsCurrentWall(int x, int y)
        {
            return CurrentWallCells.Contains(new Cell(x, y));
        }
    }
}
