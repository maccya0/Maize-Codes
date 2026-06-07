using MazeGame;
using UnityEngine;
using UnityEngine.UI;

public abstract class SliderController : MonoBehaviour
{
    [SerializeField] private InitButtonController controller;
    [SerializeField] protected Slider slider;

    protected void Register()
    {
        if (controller != null)
            controller.OnInit += InitSlider;
    }

    public abstract void UpdateSliderValue();
    public abstract void InitSlider();
    public virtual void SetupSlider()
    {
        Register();
    }
    private void OnDestroy()
    {
        if (controller != null)
            controller.OnInit -= InitSlider;
    }
}
