using UnityEngine;

namespace MazeGame
{
    public class LevelManager : MonoBehaviour
    {
        public enum Level
        {
            Level1, Level2, Level3, Level4, Level5,
            Level6, Level7, Level8, Level9, Level10
        }

        public static LevelManager Instance { get; private set; }

        [Header("レベルごとのステージデータ（インスペクターで10個登録する）")]
        [SerializeField] private MazeData[] mazeDatas;

        private Level selectLevel = Level.Level1;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public Level GetLevel() => selectLevel;
        public void SetLevel(Level level) => selectLevel = level;

        public MazeData GetCurrentMazeData()
        {
            int index = (int)selectLevel;

            if (mazeDatas != null && index >= 0 && index < mazeDatas.Length)
            {
                return mazeDatas[index];
            }

            Debug.LogError($"LevelManager: レベル {selectLevel} (Index: {index}) に対応するデータが登録されていません！");
            return null;
        }
    }
}