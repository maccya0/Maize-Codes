using System;
using System.Threading.Tasks;

namespace MazeGame
{
    public class GamePlayState : GameStateBase
    {
        public GamePlayState(SystemDirector director) : base(director) { }
        private MazeGameDirector GameDirector;
        public override async Task OnEnterState()
        {
            await GameSceneManager.Instance.LoadMaze();
            GameDirector = UnityEngine.Object.FindAnyObjectByType<MazeGameDirector>();
            if (GameDirector == null)
            {
                throw new Exception("Not Find A MazeGameDirector");
            }
            await GameDirector.Init();
        }

        public override void OnRuntimeState()
        {
            if (GameDirector != null)
            {
                GameDirector.Tick();
            }
        }

        public override async Task OnExitState()
        {
            if (GameDirector != null)
            {
                await GameDirector.Destroy();
            }
        }
    }
}