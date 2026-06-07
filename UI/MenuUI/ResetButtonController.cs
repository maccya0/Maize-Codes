using UnityEngine;
using UnityEngine.SceneManagement;


namespace MazeGame
{
    public class ResetButtonController : UICursorContoroller
    {
        [SerializeField] GameDirector gameDirector;
        [SerializeField] SoundData decideData;
        public void ActionButton()
        {
            SoundManager soundManager = SoundManager.Instance;
            if (soundManager != null)
            {
                soundManager.RequestSe(decideData, transform.position, false);
            }
            // 再度ステージを生成する
            SoundManager.Instance.StopAllSe();
            SceneManager.LoadSceneAsync("Maze", LoadSceneMode.Single);
        }

    }
}
