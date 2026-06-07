using UnityEngine;
using UnityEngine.EventSystems;

namespace MazeGame
{
    public class SelectStageButtonContoroller : UICursorContoroller
    {
        [SerializeField] GameObject ResultMenu;
        [SerializeField] GameObject StageMenu;
        [SerializeField] SoundData decideData;
        [SerializeField] GameObject levelButton;
        public void OnSelectButton()
        {
            SoundManager soundManager = SoundManager.Instance;
            if (soundManager != null)
            {
                soundManager.RequestSe(decideData, transform.position, false);
            }
            UIWindowManager.Instance.ActiveLevelUI();
            EventSystem.current.SetSelectedGameObject(levelButton);
        }
    }

}
