
using UnityEngine;
namespace MazeGame
{
    public class EnemyOnceEvent : MazeOnceEvent
    {

        [SerializeField] private int SpwnNum = 10; // •¦‚«”

        public override void TriggerEvent()
        {
            if (base.eventFlag) return;
            base.TriggerEvent();
            CreateEnemy();
        }

        private void CreateEnemy()
        {
            int max = EnemyManager.Instance.GetSpownPointNum();
            if(max < 1) return;
            if(max > SpwnNum)
            {
                max = SpwnNum;
            }
            base.OutputMessage("–‚•¨‚ª‘‚¦‚Ä‚«‚½");
            int num = UnityEngine.Random.Range(1, max);
            for(; num>0;num--)
            {
                EnemyManager.Instance.GenerateEnemy();
            }
        }
    }
}