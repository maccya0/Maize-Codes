
namespace MazeGame
{
    public class NoEvent : MazeOnceEvent
    {

        public override void TriggerEvent()
        {
            if (base.eventFlag) return;
            base.TriggerEvent();
            base.OutputMessage("‰½‚à‹N‚±‚ç‚È‚©‚Á‚½");
        }
    }
}