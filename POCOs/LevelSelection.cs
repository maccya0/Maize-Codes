using UnityEngine;

namespace MazeGame
{
    public enum GameLevel
    {
        Level1 = 1, Level2, Level3, Level4, Level5,
        Level6, Level7, Level8, Level9, Level10
    }

    public class LevelSelection
    {

        private readonly MazeData[] mazeDatas;

        private GameLevel selectedLevel;

        public LevelSelection(MazeData[] allMazeDatas)
        {
            mazeDatas = allMazeDatas;
            selectedLevel = GameLevel.Level1;
        }

        public GameLevel GetLevel() => selectedLevel;
        public void SetLevel(GameLevel level) => selectedLevel = level;

        public MazeData GetCurrentMazeData()
        {
            int index = (int)selectedLevel;

            if (mazeDatas != null && index >= 0 && index < mazeDatas.Length)
            {
                return mazeDatas[index];
            }

            Debug.LogError($"LevelManager: レベル {selectedLevel} (Index: {index}) に対応するデータが登録されていません！");
            return null;
        }
    }
}