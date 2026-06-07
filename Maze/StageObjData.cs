using UnityEngine;
using System.Collections.Generic;


namespace MazeGame
{
    [CreateAssetMenu(menuName = "GameData/StageListData")]
    class StageObjData : ScriptableObject
    {
        [SerializeField] public List<GameObject> PlaneTrapList;    // 床の罠リスト
        [SerializeField] public GameObject PlanePrehab;    // 床の通常のプレハブ
        [SerializeField] public List<GameObject> WallTrapList; // 壁の罠リスト
        [SerializeField] public List<GameObject> WallPrehabList; // 壁の通常のプレハブ
        [SerializeField] public GameObject UnBreakableWall; // 破壊不可の壁
        [SerializeField] public GameObject NormalWall; //通常の壁
        [SerializeField] public GameObject LampWall; //ランプの壁
    }

}
