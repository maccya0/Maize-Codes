
using UnityEngine;
using System.Collections;
namespace MazeGame
{
    public class TimeSlowEvent : MazeTimeEvent
    {
        [SerializeField]private GameTimeManager timeManager;
        [SerializeField] private float rate =0.5f;
        protected override void EventStart()
        {
            base.OutputMessage("ŠÔ‚Ìi‚İ‚ª’x‚­‚È‚é");
            timeManager.SetTimeAccele(rate);
        }
        protected override void EventEnd()
        {
            timeManager.ResetTimeeAccele();
            base.OutputMessage("ŠÔ‚Ìi‚İ‚ª–ß‚Á‚½");
        }

        protected override IEnumerator EventAction()
        {
            yield return new WaitForSeconds(base.EventTime);
        }
    }
}