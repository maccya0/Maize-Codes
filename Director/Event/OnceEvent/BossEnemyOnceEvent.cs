
using UnityEngine;
namespace MazeGame
{
    public class BossEnemyOnceEvent : MazeOnceEvent
    {

        [SerializeField] private GameObject bossEnemy;

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
            base.OutputMessage("‹­‚¢–‚•¨‚ªŒ»‚ê‚½");
            EnemyManager.Instance.GenerateEnemy(bossEnemy);
        }
    }
}