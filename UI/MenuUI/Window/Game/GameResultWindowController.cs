using MazeGame;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameResultWindowController : MonoBehaviour
{
    [Header("--------------Windowのボタン関連--------------")]
    [SerializeField] private Button LevelWindow;
    [SerializeField] private Button ReturnMainMenu;
    [SerializeField] private SoundData decideSound;

    [Header("--------------遷移先のボタン関連--------------")]
    [SerializeField] private GameObject Level1Button;

    [Header("--------------デリゲート関連--------------")]
    // これらは上位のDirectorから導線を引かないようにUnityEventで直接遷移先をデリゲートする
    [SerializeField] private UnityEvent onReturnMenuRequested;


    public void WindowInit()
    {
        // ボタンの初期化
        if (LevelWindow != null) LevelWindow.onClick.AddListener(ActionSelectLevel);
        if (ReturnMainMenu != null) ReturnMainMenu.onClick.AddListener(ActionReturnMenu);
    }


    public void ActionSelectLevel()
    {
        SoundManager soundManager = SoundManager.Instance;
        if (soundManager != null)
        {
            soundManager.RequestSe(decideSound, transform.position, false);
        }
        // 自身を非表示にする
        UIWindowManager.Instance.ActiveLevelUI();
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(Level1Button);

    }

    public void ActionReturnMenu()
    {
        SoundManager soundManager = SoundManager.Instance;
        if (soundManager != null)
        {
            soundManager.RequestSe(decideSound, transform.position, false);
        }

        // ゲームを終了してメニューに戻る
        onReturnMenuRequested?.Invoke();
    }
}
