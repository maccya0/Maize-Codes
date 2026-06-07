
using UnityEngine;
using System.Collections;
namespace MazeGame
{
    public class SpeedUpTimeEvent : MazeTimeEvent
    {
        [SerializeField] PlayerController player;
        [SerializeField] Transform EffectPos;
        [SerializeField] float upRate = 0.3f;
        [SerializeField] GameObject particle;
        GameObject Effect;

        protected override void EventStart()
        {
            Effect = Instantiate(particle, EffectPos);
            Effect.transform.SetParent(player.transform);
            OutputMessage("‘«Žæ‚è‚ªŒy‚­‚È‚é");
        }
        protected override void EventEnd()
        {
            Destroy(Effect);
            player.CompleateSpeedData(GetInstanceID().ToString());
            base.OutputMessage("‚¢‚Â‚à‚Ì‘«Žæ‚è‚É–ß‚Á‚½");
        }

        protected override IEnumerator EventAction()
        {
            float upVal = player.GetSpped() * upRate;
            player.AddSpeed(GetInstanceID().ToString(), upVal);
            yield return new WaitForSeconds(base.EventTime);
        }

    }
}