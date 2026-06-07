using MazeGame;
using UnityEngine;

public class BGMVolumeSliderController : SliderController
{
    [SerializeField] SoundData sliderSound;
    public override void SetupSlider()
    {
        base.SetupSlider();
        SoundManager soundManager = SoundManager.Instance;
        if (soundManager == null) return;
        slider.value = soundManager.GetBGMVolume();

    }

    public override void UpdateSliderValue()
    {
        SoundManager soundManager = SoundManager.Instance;
        if (soundManager == null) return;
        soundManager.RequestSe(sliderSound,this.transform.position,false);
        soundManager.ChangeBgmVolume(slider.value);
    }

    public override void InitSlider()
    {
        SoundManager.Instance.Initialize();
        slider.value = SoundManager.Instance.GetBGMVolume();
    }

}
