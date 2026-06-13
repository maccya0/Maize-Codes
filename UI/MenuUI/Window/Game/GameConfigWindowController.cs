using MazeGame;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameConfigWindowController : MonoBehaviour
{
    [Header("--------------Windowのボタン関連--------------")]
    [SerializeField] Button returnMenuButton;
    [SerializeField] Button returnGameButton;
    [SerializeField] Button InitializeButton;
    [SerializeField] Button explainButton;
    [SerializeField] Button retryButton;
    [SerializeField] SoundData decideSound;

    [Header("--------------遷移先のボタン関連--------------")]
    [SerializeField] GameObject explainReturnButton;

    [Header("--------------Slider関連--------------")]
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider SESlider;
    [SerializeField] Slider MasterSlider;
    [SerializeField] Slider LightSlider;
    [SerializeField] Light Light;
    [SerializeField] SoundData sliderSound;

    [Header("--------------デリゲート関連--------------")]
    // これらは上位のDirectorから導線を引かないようにUnityEventで直接遷移先をデリゲートする
    [SerializeField] private UnityEvent onReturnMenuRequested;
    [SerializeField] private UnityEvent onRetryGameRequested;
    public void WindowInit()
    {
        // スライダーの初期化
        SoundManager soundManager = SoundManager.Instance;
        if (soundManager == null) return;
        BGMSlider.value = soundManager.GetBGMVolume();
        SESlider.value = soundManager.GetSEVolume();
        MasterSlider.value = soundManager.GetMasterVolume();
        LightManager lightManager = LightManager.Instance;
        if (lightManager == null) return;
        LightSlider.value = LightManager.Instance.GetNormalizedVal();
        Light.intensity = LightManager.Instance.GetLightVal();
        if (BGMSlider != null) BGMSlider.onValueChanged.AddListener(UpdateBGMSliderValue);
        if (SESlider != null) SESlider.onValueChanged.AddListener(UpdateSESliderValue);
        if (MasterSlider != null) MasterSlider.onValueChanged.AddListener(UpdateMasterSliderValue);
        if (LightSlider != null) LightSlider.onValueChanged.AddListener(UpdateLightSliderValue);
        // ボタンの初期化
        if (returnGameButton != null) returnGameButton.onClick.AddListener(ActionReturnGame);
        if (returnMenuButton != null) returnMenuButton.onClick.AddListener(ActionReturnMenu);
        if (retryButton != null) retryButton.onClick.AddListener(ActionRetryGame);
        if (InitializeButton != null) InitializeButton.onClick.AddListener(InitSlider);
        if (explainButton != null) explainButton.onClick.AddListener(ActionExplain);
    }


    public void UpdateBGMSliderValue(float value)
    {
        if (SoundManager.Instance == null) return;
        SoundManager soundManager = SoundManager.Instance;
        soundManager.RequestSe(sliderSound, this.transform.position, false);
        soundManager.ChangeBgmVolume(value);
    }
    public void UpdateSESliderValue(float value)
    {
        if (SoundManager.Instance == null) return;
        SoundManager soundManager = SoundManager.Instance;
        soundManager.RequestSe(sliderSound, this.transform.position, false);
        soundManager.ChangeSeVolume(value);
    }
    public void UpdateMasterSliderValue(float value)
    {
        if (SoundManager.Instance == null) return;
        SoundManager soundManager = SoundManager.Instance;
        soundManager.RequestSe(sliderSound, this.transform.position, false);
        soundManager.ChangeVolume(value);
    }
    public void UpdateLightSliderValue(float value)
    {
        if (SoundManager.Instance == null) return;
        if (LightManager.Instance == null) return;
        SoundManager soundManager = SoundManager.Instance;
        soundManager.RequestSe(sliderSound, this.transform.position, false);
        LightManager.Instance.AdjustLightVal(value);
        Light.intensity = LightManager.Instance.GetLightVal();
    }

    public void InitSlider()
    {
        if (SoundManager.Instance == null) return;
        if (LightManager.Instance == null) return;
        SoundManager.Instance.Initialize();
        BGMSlider.value = SoundManager.Instance.GetBGMVolume();
        SESlider.value = SoundManager.Instance.GetSEVolume();
        MasterSlider.value = SoundManager.Instance.GetMasterVolume();
        LightManager.Instance.Initialize();
        LightSlider.value = LightManager.Instance.GetNormalizedVal();
        Light.intensity = LightManager.Instance.GetLightVal();
    }

    public void ActionReturnGame()
    {
        SoundManager soundManager = SoundManager.Instance;
        if (soundManager != null)
        {
            soundManager.RequestSe(decideSound, transform.position, false);
        }
        // 自身を非表示にする
        InputManager.Instance.ChangeInputModeUIToPlayer();
        UIWindowManager.Instance.ActiveGameUI();
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
    public void ActionRetryGame()
    {
        SoundManager soundManager = SoundManager.Instance;
        if (soundManager != null)
        {
            soundManager.RequestSe(decideSound, transform.position, false);
        }

        // ゲームを終了してメニューに戻る
        onRetryGameRequested?.Invoke();
    }

    public void ActionExplain()
    {
        SoundManager soundManager = SoundManager.Instance;
        if (soundManager != null)
        {
            soundManager.RequestSe(decideSound, this.transform.position, false);
        }
        UIWindowManager.Instance.ActiveLevelExplain();
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(explainReturnButton);
    }

}
