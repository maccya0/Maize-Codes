using UnityEngine;
using System.Collections.Generic;
using MazeGame;
using UnityEngine.EventSystems;

public class UIWindowManager : MonoBehaviour
{
    public static UIWindowManager Instance { get; private set; }
    [SerializeField] GameObject ConfigWindow;
    [SerializeField] GameObject ResultWindow;
    [SerializeField] GameObject GameUI;
    [SerializeField] GameObject LevelUI;
    [SerializeField] GameObject ExplainUI;
    [SerializeField] GameObject returnGameButton;
    [SerializeField] List<SliderController> SliderController;
    [SerializeField] GameTimeManager timeManager;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        foreach (var controller in SliderController)
        {
            controller.SetupSlider();
        }
    }

    public void ActiveConfigWindow()
    {
        timeManager.StopGame();
        ConfigWindow.SetActive(true);
        ResultWindow.SetActive(false);
        GameUI.SetActive(false);
        LevelUI.SetActive(false);
        ExplainUI.SetActive(false);
        EventSystem.current.SetSelectedGameObject(returnGameButton);
    }
    public void ActiveResultWindow()
    {
        timeManager.StopGame();
        ConfigWindow.SetActive(false);
        ResultWindow.SetActive(true);
        GameUI.SetActive(false);
        LevelUI.SetActive(false);
        ExplainUI.SetActive(false);
    }
    public void ActiveGameUI()
    {
        timeManager.StartGame();
        ConfigWindow.SetActive(false);
        ResultWindow.SetActive(false);
        GameUI.SetActive(true);
        LevelUI.SetActive(false);
        ExplainUI.SetActive(false);
    }
    public void ActiveLevelUI()
    {
        timeManager.StopGame();
        ConfigWindow.SetActive(false);
        ResultWindow.SetActive(false);
        GameUI.SetActive(false);
        LevelUI.SetActive(true);
        ExplainUI.SetActive(false);
    }
    public void ActiveLevelExplain()
    {
        timeManager.StopGame();
        ConfigWindow.SetActive(false);
        ResultWindow.SetActive(false);
        GameUI.SetActive(false);
        LevelUI.SetActive(false);
        ExplainUI.SetActive(true);
    }

}
