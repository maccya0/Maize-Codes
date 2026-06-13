using MazeGame;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuLevelWindowController : MonoBehaviour
{
    [Header("--------------Windowのボタン関連--------------")]
    [SerializeField] private Button Levele1;
    [SerializeField] private Button Levele2;
    [SerializeField] private Button Levele3;
    [SerializeField] private Button Levele4;
    [SerializeField] private Button Levele5;
    [SerializeField] private Button Levele6;
    [SerializeField] private Button Levele7;
    [SerializeField] private Button Levele8;
    [SerializeField] private Button Levele9;
    [SerializeField] private Button Levele10;
    [SerializeField] private SoundData decideSound;
    [SerializeField] private Button ReturnButton;
    [Header("--------------遷移先のボタン関連--------------")]
    [SerializeField] private GameObject StartButton;

    [Header("--------------デリゲート関連--------------")]
    // これらは上位のDirectorから導線を引かないようにUnityEventで直接遷移先をデリゲートする
    [SerializeField] private UnityEvent onStartGame;


    private LevelSelection levelSelection;
    public void WindowInit(LevelSelection _levelSelection)
    {
        levelSelection = _levelSelection;
        List<Button> levelList = new List<Button>
        {
            Levele1, Levele2, Levele3, Levele4, Levele5, 
            Levele6, Levele7, Levele8, Levele9, Levele10
        };
        // ボタンの初期化
        for (int i = 0; i < levelList.Count; i++)
        {
            Button levelButton = levelList[i];
            if (levelButton == null) continue;

            int levelIndex = i + 1;

            levelButton.onClick.AddListener(() => Selectlevel(levelIndex));
        }
        if (ReturnButton != null) ReturnButton.onClick.AddListener(ActionReturn);
    }


    public void Selectlevel(int selectLevel)
    {
        if (selectLevel < 1 || selectLevel > 10)
        {
            Debug.LogWarning($"Unuse Data Selected: {selectLevel}");
            selectLevel = 1; // 不正な値はLevel1扱いにする
        }

        GameLevel level = (GameLevel)(selectLevel);

        // 効果音の再生
        SoundManager soundManager = SoundManager.Instance;
        if (soundManager != null)
        {
            soundManager.RequestSe(decideSound, transform.position, false);
        }

        levelSelection.SetLevel(level);
        onStartGame?.Invoke();
    }
    public void ActionReturn()
    {
        SoundManager soundManager = SoundManager.Instance;
        if (soundManager != null)
        {
            soundManager.RequestSe(decideSound, transform.position, false);
        }
        MainMenuUIWindowManager.Instance.ActiveMenuWindow();
        EventSystem.current.SetSelectedGameObject(StartButton);
    }
}
