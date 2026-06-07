using MazeGame;
using UnityEngine;


public class ReturnGameButtonController : UICursorContoroller
{
    [SerializeField] private InputManager InputManager;
    [SerializeField] SoundData decideData;

    public void ActionButton()
    {
        SoundManager soundManager = SoundManager.Instance;
        if (soundManager != null)
        {
            soundManager.RequestSe(decideData, transform.position,false);
        }
        // ©g‚ğ”ñ•\¦‚É‚·‚é
        InputManager.ChangeInputModeUIToPlayer();
        UIWindowManager.Instance.ActiveGameUI();
    }

}
