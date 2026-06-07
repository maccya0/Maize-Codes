using MazeGame;
using UnityEngine;

public class MasterVolumeSliderController : SliderController
{
    [SerializeField] SoundData sliderSound;
    public override void SetupSlider()
    {
        base.SetupSlider();
        SoundManager soundManager = SoundManager.Instance;
        if (soundManager == null) return;
        slider.value = soundManager.GetMasterVolume();
    }

    public override void UpdateSliderValue()
    {
        SoundManager soundManager = SoundManager.Instance;
        if (soundManager == null) return;
        soundManager.RequestSe(sliderSound, this.transform.position, false);
        soundManager.ChangeVolume(slider.value);
    }

    public override void InitSlider()
    {
        SoundManager.Instance.Initialize();
        slider.value = SoundManager.Instance.GetMasterVolume();
    }

}
