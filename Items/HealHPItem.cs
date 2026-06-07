using UnityEngine;


namespace MazeGame
{
    [CreateAssetMenu(menuName = "GameData/HealHPItem")]
    public class HealHPItem : ItemBase
    {
        [SerializeField] private float healRate = 0.5f;

        // 抽象メソッド：アイテムごとの「固有アクション」を定義
        public override void Use(PlayerController controller, MessageScrollManager messageScrollManager)
        {
            short healVal = (short)(controller.GetMaxHP()*healRate);
            controller.HealHP(healVal);
            UseMessage(messageScrollManager);
        }
    }
}
