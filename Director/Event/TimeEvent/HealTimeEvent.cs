
using UnityEngine;
using System.Collections;
namespace MazeGame
{
    public class HealTimeEvent : MazeTimeEvent
    {
        [SerializeField] GameObject player;
        [SerializeField] Transform EffectPos;
        [SerializeField] short healVal = 2;
        [SerializeField] float healDuration = 1f;
        [SerializeField] GameObject particle;
        GameObject Effect;

        protected override void EventStart()
        {
            Effect = Instantiate(particle, EffectPos);
            Effect.transform.SetParent(player.transform);

        }
        protected override void EventEnd()
        {
            Destroy(Effect);
            OutputMessage("Œû‚ª©‘R‚ÆÇ‚ª‚ç‚È‚­‚È‚Á‚½");
        }

        protected override IEnumerator EventAction()
        {
            base.elapsedTime = 0;
            float duration = 0f;
            base.OutputMessage("Œû‚ª©‘R‚ÆÇ‚ª‚Á‚Ä‚¢‚­");
            while (base.elapsedTime <= base.EventTime)
            {
                // ¡Œã‚Ì‰ü‘PŸ‘æ‚Åplayer‚ªíœ‚³‚ê‚½ê‡‚Ì‘Î‰
                if (player == null) yield break;
                yield return null;
                base.elapsedTime += Time.deltaTime;
                duration += Time.deltaTime;
                if (duration > healDuration)
                {
                    player.GetComponent<PlayerController>().HealHP(healVal);
                    duration = 0f;
                }
            }
        }
    }
}