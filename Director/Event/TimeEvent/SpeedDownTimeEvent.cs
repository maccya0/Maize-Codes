
using UnityEngine;
using System.Collections;
namespace MazeGame
{
    public class SpeedDownTimeEvent : MazeTimeEvent
    {
        [SerializeField] GameObject player;
        [SerializeField] Transform EffectPos;
        [SerializeField] float downRate=0.5f;
        [SerializeField] GameObject particle;
        HPStatus status;
        GameObject Effect;

        protected override void EventStart()
        {
            Effect = Instantiate(particle, EffectPos);
            Effect.transform.SetParent(player.transform);
            OutputMessage("ë´éÊÇËÇ™èdÇ≠Ç»ÇÈ");
        }
        protected override void EventEnd()
        {
            Destroy(Effect);
            player.GetComponent<PlayerController>().CompleateSpeedData(GetInstanceID().ToString());
            OutputMessage("Ç¢Ç¬Ç‡ÇÃë´éÊÇËÇ…ñﬂÇ¡ÇΩ");
        }

        protected override IEnumerator EventAction()
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            float currentSpeed = playerController.GetSpped();
            float downval = currentSpeed * downRate;
            player.GetComponent<PlayerController>().DownSpeed(GetInstanceID().ToString(), -downval);
            yield return new WaitForSeconds(base.EventTime);
        }
    }
}