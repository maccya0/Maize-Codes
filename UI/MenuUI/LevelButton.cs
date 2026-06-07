using UnityEngine;
using UnityEngine.SceneManagement;

namespace MazeGame
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private SoundData deciedSound;

        public void Selectlevel(int selectLevel)
        {
            if (selectLevel < 1 || selectLevel > 10)
            {
                Debug.LogWarning($"Unuse Data Selected: {selectLevel}");
                selectLevel = 1; // 不正な値はLevel1扱いにする
            }

            LevelManager.Level level = (LevelManager.Level)(selectLevel - 1);

            // 効果音の再生
            SoundManager soundManager = SoundManager.Instance;
            if (soundManager != null)
            {
                soundManager.RequestSe(deciedSound, transform.position, false);
            }

            LevelManager.Instance.SetLevel(level);
            SoundManager.Instance.StopAllSe();
            SceneManager.LoadSceneAsync("Maze", LoadSceneMode.Single);
        }
    }
}