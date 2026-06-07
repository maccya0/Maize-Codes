using UnityEngine; // ScriptableObject‚âSprite‚É•K—v

namespace MazeGame
{
    [CreateAssetMenu(menuName = "GameData/EmptyItem")]
    public class EmptyItem : ItemBase
    {
        public override void Use(PlayerController controller, MessageScrollManager messageScrollManager)
        {
            // ‰½‚à‚µ‚È‚¢
            return;
        }
    }
}