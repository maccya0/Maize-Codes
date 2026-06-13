using MazeGame;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameExplainWindowController : MonoBehaviour
{
    [Header("--------------Windowのボタン関連--------------")]
    [SerializeField] Button returnConfigButton;
    [SerializeField] SoundData decideSound;

    [Header("--------------遷移先のボタン関連--------------")]
    [SerializeField] GameObject explainButton;

    public void WindowInit()
    {
        // ボタンの初期化
        if (returnConfigButton != null) returnConfigButton.onClick.AddListener(ActionReturnConfig);
    }



    public void ActionReturnConfig()
    {
        SoundManager soundManager = SoundManager.Instance;
        if (soundManager != null)
        {
            soundManager.RequestSe(decideSound, transform.position, false);
        }
        UIWindowManager.Instance.ActiveConfigWindow();
        EventSystem.current.SetSelectedGameObject(explainButton);
    }

}
