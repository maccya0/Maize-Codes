
using UnityEngine;

namespace MazeGame
{
    [CreateAssetMenu(fileName = "EnemyConstants", menuName = "MazeGame/Constants/Enemy")]

    public class EnemyConstants : ScriptableObject
    {
        public const float Height = 0.5f;
        public const string Tag = "Enemy";
    }
}
