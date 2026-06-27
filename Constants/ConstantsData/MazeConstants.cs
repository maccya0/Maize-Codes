
using UnityEngine;

namespace MazeGame
{
    [CreateAssetMenu(fileName = "MazeConstants", menuName = "MazeGame/Constants/Maze")]

    public class MazeConstants : ScriptableObject
    {
        // 背景との位置調整用
       [SerializeField]  public float rootX = 450f;
       [SerializeField]  public float rootY = 76f;
       [SerializeField]  public float rootZ = 220f;
       [SerializeField]  public string wallTag = "Wall";
       [SerializeField]  public string indestructibleWallTag = "Indestructible";
        public enum JudgeState
        {
            None,
            Win,
            Lose,
        }
       [SerializeField]  public float AngRange = 0.2f;
       [SerializeField]  public float ObjHeight = 1.0f;
       [SerializeField]  public float PosOffset = 1.0f;

        public enum MazeObjKinds
        {
            EPath,  //通路
            ETrapPath,  //罠の通路
            EUnBreakWall,   //破壊不可の壁
            EBreakWall, //破壊可能の壁
            ETrapWall,  //罠の壁
            EEnemyPos,  //エネミー設置個所
            EStart, //スタート位置
            EChecPoint, //チェックポイント位置
            EGoal,   //ゴール位置
            EItem,   //アイテム位置
            None
        }
        public enum Direct
        {
            North,
            East,
            South,
            West,
            Nothing,
            Siege
        };
    }
}
