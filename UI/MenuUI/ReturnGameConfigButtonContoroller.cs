using UnityEngine;

namespace MazeGame
{
    public class ReturnGameConfigButtonContoroller : UICursorContoroller
    {
        [SerializeField] UIWindowManager uIWindowManager;
        [SerializeField] SoundData decideData;
        [SerializeField] GameObject returnButton;
        public void OnSelectButton()
        {
            SoundManager soundManager = SoundManager.Instance;
            if (soundManager != null)
            {
                soundManager.RequestSe(decideData, this.transform.position, false);
            }
            uIWindowManager.ActiveConfigWindow();
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(returnButton);
        }
    }

}
