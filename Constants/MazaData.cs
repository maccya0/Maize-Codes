using UnityEngine;


namespace MazeGame
{
    [CreateAssetMenu(menuName = "GameData/MazeData")]
    public class MazeData : ScriptableObject
    {
        [SerializeField] public int StageSize = 15; // ステージサイズ
        [SerializeField] public int EnemyNum = 15;  // 敵の数
        [SerializeField] public int TrapNum = 15;   // 罠の数
        [SerializeField] public int ExtendRate = 3;   // ステージ拡張率
        [SerializeField] public int CheckPointNum = 3;   // チェックポイント数
        [SerializeField] public int ItemNum = 3;   // アイテム数
    }

}
