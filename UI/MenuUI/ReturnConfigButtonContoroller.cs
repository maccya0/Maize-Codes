using UnityEngine;

namespace MazeGame
{
    public class ReturnConfigButtonContoroller : UICursorContoroller
    {
        [SerializeField] GameObject ConfigMenu;
        [SerializeField] GameObject ExplainMenu;
        [SerializeField] SoundData decideData;
        [SerializeField] GameObject returnButton;
        public void OnSelectButton()
        {
            SoundManager soundManager = SoundManager.Instance;
            if (soundManager != null)
            {
                soundManager.RequestSe(decideData, this.transform.position, false);
            }
            ExplainMenu.SetActive(false);
            // 使い回しを考慮してStartMenuの有無でタイトル↔設定、プレイ↔設定を切り替え出来るようにする
            if (ConfigMenu != null)
            {
                ConfigMenu.SetActive(true);
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(returnButton);
            }
        }
    }

}
