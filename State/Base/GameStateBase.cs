using System.Threading.Tasks;

namespace MazeGame
{
    public abstract class GameStateBase
    {
        protected readonly SystemDirector Director;

        protected GameStateBase(SystemDirector director)
        {
            Director = director;
        }

        public abstract Task OnEnterState();

        public abstract void OnRuntimeState();

        public abstract Task OnExitState();
    }
}