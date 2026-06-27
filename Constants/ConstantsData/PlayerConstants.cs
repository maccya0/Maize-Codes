
using UnityEngine;

namespace MazeGame
{
    [CreateAssetMenu(fileName = "PlayerConstants", menuName = "MazeGame/Constants/Player")]
    public class PlayerConstants : ScriptableObject
    {
        [SerializeField] public float Height = 1.0f ;
        [SerializeField] public string Tag = "Player";
        [SerializeField] public string Layer = "Player";
    }

}
