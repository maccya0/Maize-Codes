
using UnityEngine;

namespace MazeGame
{
    [CreateAssetMenu(fileName = "GameConstant", menuName = "MazeGame/Constants/Game")]

    public class GameConstant : ScriptableObject
    {
       [SerializeField]  public float Height = 0.5f;
       [SerializeField]  public string Tag = "Enemy";
    }
}
