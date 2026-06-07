using System.Collections.Generic;
using UnityEngine;

namespace MazeGame
{
    [CreateAssetMenu(menuName = "GameData/TargetData")]
    public class TargetData : ScriptableObject
    {
        [SerializeField] public List<string> TargetList;
    }

}
