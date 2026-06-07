using UnityEngine;
using UnityEngine.SceneManagement;
namespace MazeGame
{
    public class ReturnMenuButtonController : UICursorContoroller
    {
        [SerializeField] SoundData decideData;

        public void ActionButton()
        {
            SoundManager soundManager = SoundManager.Instance;
            if (soundManager != null)
            {
                soundManager.RequestSe(decideData, transform.position, false);
            }


            // スタートメニューをロードする
            SoundManager.Instance.StopAllSe();
            SceneManager.LoadSceneAsync("StartMenu", LoadSceneMode.Single);
        }

    }
}
