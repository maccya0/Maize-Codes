
using UnityEngine;
using System.Collections;
namespace MazeGame
{
    public class PoizonTimeEvent : MazeTimeEvent
    {
        [SerializeField] GameObject player;
        [SerializeField] Transform EffectPos;
        [SerializeField] short damageVal = 10;
        [SerializeField] short healVal = 200;
        [SerializeField] GameObject particle;
        [SerializeField] float damageDuration=1f;
        GameObject Effect;

        protected override void EventStart()
        {
            Effect = Instantiate(particle, EffectPos);
            Effect.transform.SetParent(player.transform);

        }
        protected override void EventEnd()
        {
            Destroy(Effect);
            OutputMessage("ì≈Ç©ÇÁâï˙Ç≥ÇÍÇΩ");
            player.GetComponent<PlayerController>().HealHP(healVal);
        }

        protected override IEnumerator EventAction()
        {
            base.elapsedTime = 0;
            float duration = 0;
            base.OutputMessage("ì≈Ç…êIÇ‹ÇÍÇÈ");
            while (base.elapsedTime <= base.EventTime)
            {
                // ç°å„ÇÃâ¸ëPéüëÊÇ≈playerÇ™çÌèúÇ≥ÇÍÇΩèÍçáÇÃëŒâû
                if (player == null) yield break;
                yield return null;
                base.elapsedTime += Time.deltaTime;
                duration += Time.deltaTime;
                if (duration > damageDuration)
                {
                    player.GetComponent<PlayerController>().AddDamage(damageVal,false);
                    duration = 0;
                }
            }
        }
    }
}