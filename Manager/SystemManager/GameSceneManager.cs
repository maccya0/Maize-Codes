using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace MazeGame
{
    public class GameSceneManager : BaseManager<GameSceneManager>
    {
        [SerializeField] String StartScene;
        [SerializeField] String MazeScene;
        [SerializeField] String BackGroundScene;
        [SerializeField] String SystemScene;
        private string _currentLoadedSceneName;
        protected override void Awake()
        {
            base.Awake();
            if(Instance != this) return;
            _currentLoadedSceneName = "";
        }

        public override Task ManagerStartAsync()
        {
            base.ManagerStart();
            Task loadBuckGround = LoadBackgroundAndFirstScene();
            return loadBuckGround;
        }

        private async Task LoadBackgroundAndFirstScene()
        {
            var op = SceneManager.LoadSceneAsync(BackGroundScene, LoadSceneMode.Single);
            while (!op.isDone) await Task.Yield();
        }

        private async Task LoadSceneAdditive(string targetSceneName)
        {
            if (!string.IsNullOrEmpty(_currentLoadedSceneName))
            {
                var unloadOp = SceneManager.UnloadSceneAsync(_currentLoadedSceneName);
                if (unloadOp != null)
                {
                    while (!unloadOp.isDone) await Task.Yield();
                }
            }

            var loadOp = SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Additive);
            while (!loadOp.isDone) await Task.Yield();

            Scene newlyLoadedScene = SceneManager.GetSceneByName(targetSceneName);
            SceneManager.SetActiveScene(newlyLoadedScene);

            _currentLoadedSceneName = targetSceneName;
        }

        public Task LoadStartMenu() => LoadSceneAdditive(StartScene);
        public Task LoadMaze() =>  LoadSceneAdditive(MazeScene);

        public async Task UnLoadAllScene()
        {
            string[] SceneNameList =
            {
                StartScene,
                MazeScene,
                BackGroundScene
            };

            for (int i = 0; i < SceneNameList.Length; i++)
            {
                if (!string.IsNullOrEmpty(SceneNameList[i]))
                {
                    var unloadOp = SceneManager.UnloadSceneAsync(SceneNameList[i]);
                    if (unloadOp != null)
                    {
                        while (!unloadOp.isDone) await Task.Yield();
                    }
                }
            }
        }
    }
}
