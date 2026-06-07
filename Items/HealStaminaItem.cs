
using UnityEngine;

namespace MazeGame
{
    [CreateAssetMenu(menuName = "GameData/HealStaminaItem")]
    public class HealStaminaItem : ItemBase
    {
        public override void Use(PlayerController controller, MessageScrollManager messageScrollManager)
        {
            float MaxStamina = controller.GetMaxStamina();
            controller.HealStamina(MaxStamina);
            UseMessage(messageScrollManager);
        }
    }
}
