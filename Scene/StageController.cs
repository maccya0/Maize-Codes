using UnityEngine.SceneManagement;
using UnityEngine;

public class StageController : MonoBehaviour
{
    private string currentStageName;
    public static StageController Instance { get; private set; }


    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 個別にステージを切り替えるメソッド
    public void LoadScene(string stageName)
    {
        var op = SceneManager.LoadSceneAsync(stageName, LoadSceneMode.Additive);

        op.completed += (o) =>
        {
            // 2. 新しいシーンのロードが完了してから、古いシーンを消す
            if (!string.IsNullOrEmpty(currentStageName))
            {
                SceneManager.UnloadSceneAsync(currentStageName);
            }
            currentStageName = stageName;
        };
    }

    public void LoadStartMenu()
    {
        LoadScene("StartMenu");
    }
    public void LoadBackGround()
    {
        LoadScene("BackGround");
    }

    public void LoadGame()
    {
        LoadScene("Maze");
    }
}