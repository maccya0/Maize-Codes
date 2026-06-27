using System.Collections.Generic;
using UnityEngine;

namespace MazeGame
{
    public class DiggingHoleMaze
    {
        protected enum Direction { EUp, ERight, EDown, ELeft }
        protected struct Cell
        {
            public int X { get; set; }
            public int Y { get; set; }
            public Cell(int x, int y) { this.X = x; this.Y = y; }
        }

        protected int size;
        protected System.Random random;
        private MazeConstants.MazeObjKinds[,] maze;
        private Stack<Cell> currentWallCells;
        private List<Cell> startCells;

        private bool[,] isCurrentWallMap;

        private readonly Direction[] directionPool = new Direction[4];

        public virtual void SetMazeData(int _size)
        {
            if (_size % 2 == 0) _size++;
            this.size = _size;
            this.maze = new MazeConstants.MazeObjKinds[this.size, this.size];
            this.isCurrentWallMap = new bool[this.size, this.size];
            this.startCells = new List<Cell>();
            this.currentWallCells = new Stack<Cell>();
            this.random = new System.Random();
        }

        protected MazeConstants.MazeObjKinds[,] CreateMaze()
        {
            for (int y = 0; y < this.size; y++)
            {
                for (int x = 0; x < this.size; x++)
                {
                    if (x == 0 || y == 0 || x == this.size - 1 || y == this.size - 1)
                    {
                        this.maze[x, y] = MazeConstants.MazeObjKinds.EUnBreakWall;
                    }
                    else
                    {
                        this.maze[x, y] = MazeConstants.MazeObjKinds.EPath;
                        if (x % 2 == 0 && y % 2 == 0)
                        {
                            startCells.Add(new Cell(x, y));
                        }
                    }
                }
            }

            while (startCells.Count > 0)
            {
                int index = random.Next(startCells.Count);
                Cell cell = startCells[index];
                startCells.RemoveAt(index);

                int x = cell.X;
                int y = cell.Y;

                if (this.maze[x, y] == MazeConstants.MazeObjKinds.EPath)
                {
                    currentWallCells.Clear();
                    System.Array.Clear(isCurrentWallMap, 0, isCurrentWallMap.Length);

                    ExecuteExtendWallLoop(x, y);
                }
            }
            return this.maze;
        }

        private void ExecuteExtendWallLoop(int startX, int startY)
        {
            int x = startX;
            int y = startY;

            while (true)
            {
                int dirCount = 0;
                if (this.maze[x, y - 1] == MazeConstants.MazeObjKinds.EPath && !isCurrentWallMap[x, y - 2])
                    directionPool[dirCount++] = Direction.EUp;
                if (this.maze[x + 1, y] == MazeConstants.MazeObjKinds.EPath && !isCurrentWallMap[x + 2, y])
                    directionPool[dirCount++] = Direction.ERight;
                if (this.maze[x, y + 1] == MazeConstants.MazeObjKinds.EPath && !isCurrentWallMap[x, y + 2])
                    directionPool[dirCount++] = Direction.EDown;
                if (this.maze[x - 1, y] == MazeConstants.MazeObjKinds.EPath && !isCurrentWallMap[x - 2, y])
                    directionPool[dirCount++] = Direction.ELeft;

                if (dirCount > 0)
                {
                    SetWall(x, y);

                    var dirIndex = random.Next(dirCount);
                    var chosenDir = directionPool[dirIndex];
                    var isPath = false;

                    switch (chosenDir)
                    {
                        case Direction.EUp:
                            isPath = (this.maze[x, y - 2] == MazeConstants.MazeObjKinds.EPath);
                            SetWall(x, --y); SetWall(x, --y);
                            break;
                        case Direction.ERight:
                            isPath = (this.maze[x + 2, y] == MazeConstants.MazeObjKinds.EPath);
                            SetWall(++x, y); SetWall(++x, y);
                            break;
                        case Direction.EDown:
                            isPath = (this.maze[x, y + 2] == MazeConstants.MazeObjKinds.EPath);
                            SetWall(x, ++y); SetWall(x, ++y);
                            break;
                        case Direction.ELeft:
                            isPath = (this.maze[x - 2, y] == MazeConstants.MazeObjKinds.EPath);
                            SetWall(--x, y); SetWall(--x, y);
                            break;
                    }

                    if (!isPath)
                    {
                        ClearCurrentWallMap();
                        break;
                    }
                }
                else
                {
                    // 全方向が自傷（自分の壁）になる場合、バックトラック（引き返す）
                    if (currentWallCells.Count > 0)
                    {
                        var beforeCell = currentWallCells.Pop();
                        // 元いた場所のフラグを折る
                        isCurrentWallMap[x, y] = false;

                        x = beforeCell.X;
                        y = beforeCell.Y;
                    }
                    else
                    {
                        // 引き返す場所もなくなったら終了
                        break;
                    }
                }
            }
        }

        private void SetWall(int x, int y)
        {
            this.maze[x, y] = MazeConstants.MazeObjKinds.EBreakWall;
            if (x % 2 == 0 && y % 2 == 0)
            {
                currentWallCells.Push(new Cell(x, y));
                isCurrentWallMap[x, y] = true;
            }
        }

        private void ClearCurrentWallMap()
        {
            foreach (var cell in currentWallCells)
            {
                isCurrentWallMap[cell.X, cell.Y] = false;
            }
        }
    }
}