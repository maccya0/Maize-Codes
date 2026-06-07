using UnityEngine;
using UnityEngine.SceneManagement;

public class TestMazeSceneReader : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        SceneManager.LoadSceneAsync("Maze", LoadSceneMode.Single);
    }
}
