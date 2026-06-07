using UnityEngine;

namespace MazeGame
{
    [CreateAssetMenu(menuName = "GameData/StatusData")]
    public class StatusData : ScriptableObject
    {
        public short MAXHP;
        public float MAXSTAMINA;
        public float MINSPEED;
        public float MAXSPEED;
        public float INITIALSPEED;
    }

}

