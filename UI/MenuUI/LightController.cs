using MazeGame;
using UnityEngine;

public class LightController : SliderController
{
    [SerializeField] Light Light;
    [SerializeField] SoundData sliderSound;

    public override void SetupSlider()
    {
        base.SetupSlider();
        // ‰Šú’l‚Éƒ‰ƒCƒg‚ğİ’è‚·‚é
        base.slider.value = LightManager.Instance.GetNormalizedVal();
        Light.intensity = LightManager.Instance.GetLightVal();
    }

    public override void UpdateSliderValue()
    {
        SoundManager soundManager = SoundManager.Instance;
        if (soundManager == null) return;
        soundManager.RequestSe(sliderSound, this.transform.position, false);
        LightManager.Instance.AdjustLightVal(base.slider.value);
        Light.intensity = LightManager.Instance.GetLightVal();
    }
    public override void InitSlider()
    {
        LightManager.Instance.Initialize();
        base.slider.value = LightManager.Instance.GetNormalizedVal();
        Light.intensity = LightManager.Instance.GetLightVal();
    }

}