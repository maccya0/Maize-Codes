using UnityEngine;

namespace MazeGame
{
    public class ExplainGameButtonContoroller : UICursorContoroller
    {
        [SerializeField] private UIWindowManager uIWindowManager;
        [SerializeField] SoundData decideData;
        [SerializeField] GameObject returnButton;
        public void OnSelectButton()
        {
            SoundManager soundManager = SoundManager.Instance;
            if (soundManager != null)
            {
                soundManager.RequestSe(decideData, this.transform.position, false);
            }
            uIWindowManager.ActiveLevelExplain();
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(returnButton);
        }
    }

}
