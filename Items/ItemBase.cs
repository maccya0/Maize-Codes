using UnityEngine;

namespace MazeGame
{
    public abstract class ItemBase : ScriptableObject
    {
        [SerializeField] public string itemName;
        [SerializeField] public Sprite icon;
        [SerializeField] public int weight = 1;
        protected  MessageScrollManager messageScrollManager;

        public abstract void Use(PlayerController controller, MessageScrollManager messageScrollManager);

        protected void UseMessage(MessageScrollManager messageScrollManager)
        {
            messageScrollManager.EnqueueMessage($"{itemName}‚ðŽg—p‚µ‚½");
        }

    }
}
