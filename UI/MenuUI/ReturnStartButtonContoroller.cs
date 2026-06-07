using UnityEngine;

namespace MazeGame
{
    public class ReturnStartButtonController : UICursorContoroller
    {
        [SerializeField] GameObject LevelMenu;
        [SerializeField] GameObject StartMenu;
        [SerializeField] SoundData decideData;
        public void OnSelectButton()
        {
            SoundManager soundManager = SoundManager.Instance;
            if (soundManager != null)
            {
                soundManager.RequestSe(decideData, this.transform.position, false);
            }
            LevelMenu.SetActive(false);
            // 使い回しを考慮してStartMenuの有無でタイトル↔設定、プレイ↔設定を切り替え出来るようにする
            if (StartMenu != null)
            {
                StartMenu.SetActive(true);
                GameObject firstSelect = StartMenu.transform.Find("MenuPivot/StartButton").gameObject;
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(firstSelect);
            }
        }
    }

}
