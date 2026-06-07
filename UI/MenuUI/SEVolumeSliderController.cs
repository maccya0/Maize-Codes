using MazeGame;
using UnityEngine;

public class SEVolumeSliderController : SliderController
{
    [SerializeField] SoundData slideSound;
    public override void SetupSlider()
    {
        base.SetupSlider();
        SoundManager soundManager = SoundManager.Instance;
        if(soundManager == null) return;
        slider.value = soundManager.GetSEVolume();
    }

    public override void UpdateSliderValue()
    {

        SoundManager soundManager = SoundManager.Instance;
        if (soundManager == null) return;
        soundManager.RequestSe(slideSound, this.transform.position, false);
        soundManager.ChangeSeVolume(slider.value);
    }

    public override void InitSlider()
    {
        SoundManager.Instance.Initialize();
        slider.value = SoundManager.Instance.GetSEVolume();
    }

}
