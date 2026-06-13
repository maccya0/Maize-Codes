using System.Threading.Tasks;

namespace MazeGame
{
    public class EndState : GameStateBase
    {
        public EndState(SystemDirector director) : base(director) { }

        public override async Task OnEnterState()
        {
            await Director.TransitionToDestroyAsyncInternal();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif        
        }

        public override void OnRuntimeState()
        {
            // 終了するので不要
        }

        public override Task OnExitState()
        {
            // 終了するので不要
            return Task.CompletedTask;
        }
    }
}