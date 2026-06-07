
using UnityEngine;

namespace MazeGame
{
    [CreateAssetMenu(menuName = "GameData/HealAllItem")]
    public class HealAllItem : ItemBase
    {
        [SerializeField] private int skillHealVal = 1;
        [SerializeField] private float healRate = 0.5f;
        public override void Use(PlayerController controller, MessageScrollManager messageScrollManager)
        {
            short healVal = (short)(controller.GetMaxHP() * healRate);
            controller.HealHP(healVal);
            controller.HealSkill(skillHealVal);
            float MaxStamina = controller.GetMaxStamina();
            controller.HealStamina(MaxStamina);
            UseMessage(messageScrollManager);
        }
    }
}
