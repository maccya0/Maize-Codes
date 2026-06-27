
using UnityEngine;

namespace MazeGame
{
    [CreateAssetMenu(fileName = "InputConstants", menuName = "MazeGame/Constants/Input")]

    public class InputConstants : ScriptableObject
    {
       [SerializeField]  public float deadInputVal = 0.4f;
    }
}
