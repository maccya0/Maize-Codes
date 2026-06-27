using UnityEngine;

namespace MazeGame
{
    public class MazeGameConstants : MonoBehaviour
    {
        [Header("-------------- インスペクターで設定する用 --------------")]
        [SerializeField] private PlayerConstants _playerConstants;
        [SerializeField] private EnemyConstants _enemyConstants;
        [SerializeField] private InputConstants _inputConstants;
        [SerializeField] private GameConstant _gameConstant;
        [SerializeField] private MazeConstants _mazeConstants;

        public static PlayerConstants PlayerConstants;
        public static EnemyConstants EnemyConstants;
        public static InputConstants InputConstants;
        public static GameConstant GameConstant;
        public static MazeConstants MazeConstants;

        public void Init()
        {
            PlayerConstants = _playerConstants;
            EnemyConstants = _enemyConstants;
            InputConstants = _inputConstants;
            GameConstant = _gameConstant;
            MazeConstants = _mazeConstants;
        }
    }
}