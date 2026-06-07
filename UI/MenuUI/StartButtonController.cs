using MazeGame;
using UnityEngine;
using static UIButtonInterFace;

public class StartButtonController : MonoBehaviour , UIButtonInterFace
{
    private ButtonState state;
    [SerializeField] SoundData decideData;
    [SerializeField] GameObject levelWindow;
    [SerializeField] GameObject menuWindow;
    [SerializeField] GameObject level1Button;
    public void SetState(ButtonState state)
    {
        this.state = state;
    }
    public ButtonState GetState()
    {
        return this.state;
    }

    public void ActionButton()
    {
        SoundManager soundManager = SoundManager.Instance;
        if (soundManager != null)
        {
            soundManager.RequestSe(decideData, transform.position, false);
        }
        // ÉQÅ[ÉÄäJén
        levelWindow.gameObject.SetActive(true);
        menuWindow.gameObject.SetActive(false);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(level1Button);

    }

}
