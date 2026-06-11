
using UnityEngine;
namespace MazeGame
{
    public class MazeOnceEvent : MonoBehaviour , MazeEvent
    {
        [SerializeField] private MessageScrollManager messageScrollManager;
        protected bool eventFlag;
        [SerializeField] public int rateVal = 10;
        public virtual void TriggerEvent()
        {
            if(eventFlag) return;
            eventFlag = true;
        }

        public void OutputMessage(string message)
        {
            Debug.Log(message);
            messageScrollManager.EnqueueMessage(message);

        }
    }
}