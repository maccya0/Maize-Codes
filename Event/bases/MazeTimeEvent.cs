
using UnityEngine;
using System.Collections;
namespace MazeGame
{
    public abstract class MazeTimeEvent : MonoBehaviour , MazeEvent
    {
        [SerializeField] protected float EventTime = 20f;
        [SerializeField] private MessageScrollManager messageScrollManager;
        [SerializeField] public int rateVal = 10;
        protected float elapsedTime = 0;
        protected bool eventFlag;

        public virtual void TriggerEvent()
        {
            // 現在イベント実行中なら返す
            if (eventFlag) return;
            elapsedTime = 0;
            StartCoroutine(EventFlow());
        }

        protected IEnumerator EventFlow()
        {
            eventFlag = true;
            EventStart();
            yield return EventAction();
            EventEnd();
            eventFlag = false;

        }

        protected abstract void EventStart();
        protected abstract void EventEnd();

        protected virtual IEnumerator EventAction()
        {
            yield return new WaitForSeconds(EventTime);
        }

        public void OutputMessage(string message)
        {
            Debug.Log(message);
            messageScrollManager.EnqueueMessage(message);

        }
    }
}