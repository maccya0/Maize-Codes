
using System.Collections.Generic;
using UnityEngine;
namespace MazeGame
{
    public class SkillOnceEvent : MazeOnceEvent
    {

        [SerializeField] private List<SkillAction> actionList;
        [SerializeField] private int ChargeNum = 3;

        public override void TriggerEvent()
        {
            if (base.eventFlag) return;
            base.TriggerEvent();
            AddSkill();
        }

        private void AddSkill()
        {
            base.OutputMessage("ƒXƒLƒ‹‰ñ”‚ª‘‚¦‚½");
            int Index = UnityEngine.Random.Range(1, actionList.Count);
            byte num = (byte)UnityEngine.Random.Range(1, ChargeNum);
            actionList[Index].UseCharge(num);
        }
    }
}