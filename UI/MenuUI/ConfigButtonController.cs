using MazeGame;
using UnityEngine;
using static UIButtonInterFace;

public class ConfigButtonController : MonoBehaviour , UIButtonInterFace
{
    [SerializeField] GameObject configUI;
    [SerializeField] GameObject menuUI;
    [SerializeField] SoundData decideData;
    private ButtonState state;
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
        configUI.SetActive(true);
        menuUI.SetActive(false);
        GameObject firstItem = configUI.transform.Find("Buttons/InitialButton").gameObject;

        if (UnityEngine.EventSystems.EventSystem.current != null)
        {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(firstItem);
        }
    }

}
