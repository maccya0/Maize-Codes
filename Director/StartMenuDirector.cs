using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using MazeGame;

public class StartMenuDirector : MonoBehaviour
{
    [SerializeField] List<SliderController> sliderList;
    [SerializeField] SoundData menuBGM;

    private void Awake()
    {
        if (!SceneManager.GetSceneByName("BackGroundScene").isLoaded)
        {
            SceneManager.LoadSceneAsync("BackGroundScene", LoadSceneMode.Additive);
        }
    }
    private void Start()
    {
        foreach (var slider in sliderList)
        {
            slider.SetupSlider();
        }
        SoundManager.Instance.StartBgm(menuBGM);
    }
}
