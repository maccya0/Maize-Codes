using UnityEngine;
using System.Collections.Generic;


namespace MazeGame
{
    [CreateAssetMenu(menuName = "GameData/EnemyListData")]
    class EnemyObjData : ScriptableObject
    {
        [SerializeField] public List<GameObject> enemyList;    // “G‚ÌƒŠƒXƒg
    }
}
