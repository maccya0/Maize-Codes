using System;
using Unity.AI.Navigation;
using UnityEngine;
using static MazeGame.MazeGameConstants.MazeConstants;
using static MazeGame.MazeGameConstants;

namespace MazeGame
{
    public class StageCreate : MonoBehaviour
    {

        [SerializeField] private StageObjData stageObjData;
        [SerializeField] public NavMeshSurface navMesh;
        [SerializeField] public GameObject goalObject; 
        [SerializeField] public GameObject startObject;
        [SerializeField] private GameObject[] checkPoints;
        [SerializeField] private GameObject itemBox;
        [SerializeField][Range(0, 255)] private uint enemyRange = 192;

        /* 処理時共通データ */
        private int Size;
        private MazeObjKinds[,] mazeData;
        private Maze maze;
        private GameObject rootObject;
        private Vector3 rootPos = new Vector3(MazeConstants.rootX, MazeConstants.rootY, MazeConstants.rootZ);

        public void Initialize()
        {
            ResetStage();
            if (navMesh == null)
            {
                throw new InvalidOperationException("ターゲットが未設定");
            }

            //迷路生成データ取得
            maze = Maze.Instance;
            Size = maze.GetStageSize();
            mazeData = maze.GetMazeData();
            //迷路生成
            CreateMazeStage();

            // 生成後情報格納
            maze.startTransform = startObject.transform;
            maze.goalTransform = goalObject.transform;
        }

        public void ResetStage()
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
            rootObject.name = "MazeRoot";
            rootObject.transform.position = rootPos;

            // 迷路情報削除
            maze = Maze.Instance;

        }



        private void CreateMazeStage()
        {
            for (int cloop = 0; cloop < Size; cloop++)
            {
                for (int rloop = 0; rloop < Size; rloop++)
                {
                    byte rand = (byte)UnityEngine.Random.Range(0, 255);
                    CreatePlane(cloop, rloop, rand);    //床の生成

                }
            }
            for (int cloop = 0; cloop < Size; cloop++)
            {
                for (int rloop = 0; rloop < Size; rloop++)
                {
                    byte rand = (byte)UnityEngine.Random.Range(0, 255);
                    CreateAlphaWall(cloop, rloop); //透明な壁外周に生成
                    CreateWall(cloop, rloop, rand); //壁の生成
                    CreateItem(cloop, rloop);  // アイテムの生成
                }
            }
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
                    if(mazeData[cloop, rloop] == MazeObjKinds.EEnemyPos)
                    {
                        EnemyManager.Instance.RegisterSpownPoints(maze.stageObjects[cloop, rloop].transform.position);
                        num++;
                    }
                }
            }
            // エネミーの生成
            CreateEnemy(num);

        }

        //床の生成
        private void CreatePlane(int column, int row, byte rand)
        {
            GameObject plane;
            //迷路の生成データがトラップ床以外を指している場合
            if (mazeData[column, row] != MazeObjKinds.ETrapPath)
            {
                plane = Instantiate(stageObjData.PlanePrehab);

            }
            else
            {
                plane = Instantiate(stageObjData.PlaneTrapList[(rand % stageObjData.PlaneTrapList.Count)]);
            }
            plane.transform.position = new Vector3(column * PosOffset, 0, row * PosOffset);
            SetStageInfo(plane, column, row);
        }

        //壁の生成
        private void CreateWall(int column, int row, byte rand)
        {
            //壁以外の時は何もしない
            if (!(mazeData[column, row] == MazeObjKinds.EBreakWall || mazeData[column, row] == MazeObjKinds.ETrapWall || mazeData[column, row] == MazeObjKinds.EUnBreakWall)) return;
            GameObject wall = null;

            //四隅は破壊不可の壁orステージギミック
            if (row == 0 || row == Size - 1 || column == 0 || column == Size - 1)
            {
                wall = Instantiate(stageObjData.UnBreakableWall);
                wall.transform.position = new Vector3(column * PosOffset, 0, row * PosOffset);
                wall.tag = "Indestructible";
            }
            else
            {
                //破壊可能の壁ならそのまま生成
                if (mazeData[column, row] == MazeObjKinds.EBreakWall)
                {
                    Direct direct = maze.JudeAround(column, row);
                    //周囲が壁になっている時は通常の壁
                    if (direct == Direct.Siege)
                    {
                        wall = Instantiate(stageObjData.NormalWall);
                    }
                    else if (maze.JudgeDeadEnd(column, row, Maze.CheckSize.Around) || maze.JudgeTJunction(column, row, Maze.CheckSize.Around) || maze.JudgeCorner(column, row, Maze.CheckSize.Around))
                    {
                        //終わりの時はライトを生成
                        wall = Instantiate(stageObjData.LampWall);
                        if (direct == Direct.South)
                        {
                            wall.transform.Rotate(new Vector3(0, 270, 0), Space.World);
                        }
                        else if (direct == Direct.North)
                        {
                            wall.transform.Rotate(new Vector3(0, 90, 0), Space.World);
                        }
                        else if (direct == Direct.East)
                        {
                            wall.transform.Rotate(new Vector3(0, 0, 0), Space.World);
                        }
                        else if (direct == Direct.West)
                        {
                            wall.transform.Rotate(new Vector3(0, 180, 0), Space.World);
                        }
                    }
                    else
                    {
                        //ランダムで壁を生成する
                        byte temp = (byte)UnityEngine.Random.Range(2, stageObjData.WallPrehabList.Count);
                        wall = Instantiate(stageObjData.WallPrehabList[temp]);
                        if (direct == Direct.South || direct == Direct.North)
                        {
                            wall.transform.Rotate(new Vector3(0, 90, 0), Space.World);

                        }
                        else
                        {
                            wall.transform.Rotate(new Vector3(0, 0, 0), Space.World);
                        }
                    }
                    wall.transform.position = new Vector3(column * PosOffset,0, row * PosOffset);
                    wall.tag = "Wall";
                }
                //罠の場合
                else if (mazeData[column, row] == MazeObjKinds.ETrapWall)
                {
                    Direct direct = maze.JudeAround(column, row);
                    //四方のどこかの向きor設定できる向きがなくなるまで乱数で向きを決定する
                    if (direct == Direct.Siege)
                    {
                        //前後左右が壁で設定出来る向きがない
                        wall = Instantiate(stageObjData.WallPrehabList[0]);
                        wall.transform.position = new Vector3(column * PosOffset, 0, row * PosOffset);
                        wall.tag = "Wall";
                    }
                    else
                    {
                        //見つかった方向で設定する
                        wall = Instantiate(stageObjData.WallTrapList[rand % stageObjData.WallTrapList.Count]);
                        wall.transform.position = new Vector3(column * PosOffset, 0, row * PosOffset);
                        if (direct == Direct.South)
                        {
                            wall.transform.Rotate(new Vector3(0, 270, 0), Space.World);
                        }
                        else if (direct == Direct.North)
                        {
                            wall.transform.Rotate(new Vector3(0, 90, 0), Space.World);
                        }
                        else if (direct == Direct.East)
                        {
                            wall.transform.Rotate(new Vector3(0, 0, 0), Space.World);
                        }
                        else if (direct == Direct.West)
                        {
                            wall.transform.Rotate(new Vector3(0, 180, 0), Space.World);
                        }
                        wall.GetComponent<BombBlock>().SetBombInfo(direct);
                        wall.tag = "Wall";

                    }
                }
            }
            SetStageInfo(wall, column, row);
        }
        private void CreateAlphaWall(int column, int row)
        {
            //四隅で生成
            if (row == 0 || row == Size - 1 || column == 0 || column == Size - 1)
            {
                GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                wall.GetComponent<Renderer>().enabled = false;
                wall.transform.localPosition = new Vector3(column * PosOffset, StageHeightLimit, row * PosOffset);
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
            wall.transform.localPosition = new Vector3((float)((Size - PosOffset) / 2), StageHeightLimit, (float)((Size - PosOffset) / 2));
            wall.tag = "Indestructible";
            MoveCenterPosition(wall);
            wall.transform.SetParent(rootObject.transform, false);
        }
        //敵の生成
        private void CreateEnemy(int generateNum)
        {
            LevelManager.Level level = LevelManager.Instance.GetLevel();
            int generateEnemy = (int)level/2 + 2;
            // 最低限はレベルに合わせて生成させる
            for(; generateEnemy > 0; generateEnemy--)
            {
                generateNum--;
                EnemyManager.Instance.GenerateEnemy();
            }

            for (;generateNum>0;generateNum--)
            {
                int randam = UnityEngine.Random.Range(0, 255);
                if(randam < enemyRange)
                {
                    EnemyManager.Instance.GenerateEnemy();
                }
                else
                {
                    EnemyManager.Instance.GenerateStatue();
                }
            }
        }

        // アイテムの生成
        private void CreateItem(int column, int row)
        {
            //敵の生成位置のみ処理
            if (mazeData[column, row] != MazeObjKinds.EItem) return;

            GameObject item;
            item = Instantiate(itemBox);
            item.transform.position = new Vector3(column * PosOffset, 0, row * PosOffset);
            MoveCenterPosition(item);
            item.transform.SetParent(rootObject.transform, false);

        }
        private void CreateStageGimic()
        {
            for (int row = 0; row < Size - 1; row++)
            {
                if (mazeData[Size - 1, row] == MazeObjKinds.EGoal)
                {
                    goalObject.transform.position = new Vector3((Size - 1) * PosOffset + 4.5f, 0, (row + 1) * PosOffset + 0.35f) + rootPos;
                    goalObject.transform.Rotate(new Vector3(0, 180, 0));
                    MoveCenterPosition(goalObject);
                    break;
                }
            }
            for (int row = 0; row < Size - 1; row++)
            {
                if (mazeData[0, row] == MazeObjKinds.EStart)
                {
                    startObject.transform.position = new Vector3(0 * PosOffset - 4.5f, 0, (row + 1) * PosOffset - 0.35f) + rootPos;
                    startObject.transform.Rotate(new Vector3(0, 0, 0));
                    MoveCenterPosition(startObject);
                    break;
                }
            }
            int index = 0;
            for (int column = 0; column < Size - 1; column++)
            {
                if (mazeData[column, 0] == MazeObjKinds.EChecPoint)
                {
                    checkPoints[index].transform.position = new Vector3((column + 1) * PosOffset + 0.35f , 0, 0 * PosOffset - 4.5f) + rootPos;
                    checkPoints[index].transform.Rotate(new Vector3(0, 270, 0));
                    MoveCenterPosition(checkPoints[index]);
                    index++;
                    column += maze.GetRate();
                }
                if (index == checkPoints.Length)
                {
                    break;
                }
            }
            for (int column = 0; column < Size - 1; column++)
            {
                if (mazeData[column, Size - 1] == MazeObjKinds.EChecPoint)
                {
                    checkPoints[index].transform.position = new Vector3((column + 1) * PosOffset - 0.35f, 0, (Size - 1) * PosOffset + 4.5f) + rootPos;
                    checkPoints[index].transform.Rotate(new Vector3(0, 90, 0));
                    MoveCenterPosition(checkPoints[index]);
                    index++;
                    column += maze.GetRate();
                }
                if (index == checkPoints.Length)
                {
                    break;
                }
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
            // ルート基準に移動
            //target.transform.position += rootPos;

        }

        private void SetStageInfo(GameObject stageObject ,int column, int row)
        {
            MoveCenterPosition(stageObject);
            stageObject.transform.SetParent(rootObject.transform,false);
            maze.stageObjects[column, row] = stageObject;
        }


    }
}
