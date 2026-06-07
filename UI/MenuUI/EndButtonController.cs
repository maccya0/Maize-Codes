using MazeGame;
using UnityEngine;
using static UIButtonInterFace;

public class EndButtonController : MonoBehaviour , UIButtonInterFace
{
    private ButtonState state;
    [SerializeField] SoundData decideData;
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
            soundManager.RequestSe(decideData, transform.position,false);
        }

        // ゲーム終了
        Application.Quit();

        // エディタでの停止
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
