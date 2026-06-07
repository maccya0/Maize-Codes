using UnityEngine;


namespace MazeGame
{
    public class SlipThroughGauge : SkillGauge
    {
        [SerializeField] private SlipThroughAction slipThroughAction;
        private void Start()
        {
            slipThroughAction.OnRecastProgress += SetGauge;
            slipThroughAction.OnActionSkill += SetCount;
        }

        private void OnDestroy()
        {
            slipThroughAction.OnRecastProgress -= SetGauge;
            slipThroughAction.OnActionSkill -= SetCount;
        }
    }

}
