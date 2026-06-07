using UnityEngine;

namespace MazeGame
{
    public class ReturnButtonController : UICursorContoroller
    {
        [SerializeField] GameObject ConfigMenu;
        [SerializeField] GameObject StartMenu;
        [SerializeField] SoundData decideData;
        public void OnSelectButton()
        {
            SoundManager soundManager = SoundManager.Instance;
            if (soundManager != null)
            {
                soundManager.RequestSe(decideData, this.transform.position, false);
            }
            ConfigMenu.SetActive(false);
            // 使い回しを考慮してStartMenuの有無でタイトル↔設定、プレイ↔設定を切り替え出来るようにする
            if (StartMenu != null)
            {
                StartMenu.SetActive(true);
                // StartMenu内の特定のボタン（例：ConfigButton）を強制的に選択状態にする
                GameObject firstSelect = StartMenu.transform.Find("MenuPivot/ConfigButton").gameObject;
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(firstSelect);
            }
        }
    }

}
