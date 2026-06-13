using System;
using System.Threading.Tasks;

namespace MazeGame
{
    public class StartMenuState : GameStateBase
    {
        public StartMenuState(SystemDirector director) : base(director) { }
        private StartMenuDirector StartMenuDirector;
        public override async Task OnEnterState()
        {
            await GameSceneManager.Instance.LoadStartMenu();
            StartMenuDirector = UnityEngine.Object.FindAnyObjectByType<StartMenuDirector>();
            if (StartMenuDirector == null)
            {
                throw new Exception("Not Find A StartMenuDirector");
            }
            await StartMenuDirector.Init();
        }

        public override void OnRuntimeState()
        {
            if(StartMenuDirector != null)
            {
                StartMenuDirector.Tick();
            }
        }

        public override async Task OnExitState()
        {
            if (StartMenuDirector != null)
            {
                await StartMenuDirector.Destroy();
            }
        }
    }
}