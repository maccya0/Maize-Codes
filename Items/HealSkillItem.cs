
using UnityEngine;

namespace MazeGame
{
    [CreateAssetMenu(menuName = "GameData/HealSkillItem")]
    public class HealSkillItem : ItemBase
    {
        [SerializeField] int healVal = 1;
        public override void Use(PlayerController controller, MessageScrollManager messageScrollManager)
        {
            controller.HealSkill(healVal);
            UseMessage(messageScrollManager);
        }
    }
}
