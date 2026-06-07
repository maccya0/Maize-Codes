using UnityEngine;

namespace MazeGame
{
    public class ExplainButtonContoroller : UICursorContoroller
    {
        [SerializeField] GameObject ControllerMenu;
        [SerializeField] GameObject ConfigMenu;
        [SerializeField] SoundData decideData;
        [SerializeField] GameObject returnButton;
        public void OnSelectButton()
        {
            SoundManager soundManager = SoundManager.Instance;
            if (soundManager != null)
            {
                soundManager.RequestSe(decideData, this.transform.position, false);
            }
            ConfigMenu.SetActive(false);
            // 使い回しを考慮してStartMenuの有無でタイトル↔設定、プレイ↔設定を切り替え出来るようにする
            if (ControllerMenu != null)
            {
                ControllerMenu.SetActive(true);
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(returnButton);
            }
        }
    }

}
