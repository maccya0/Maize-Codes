using UnityEngine;
namespace MazeGame
{
    public class HealGauge : SkillGauge
    {
        [SerializeField] private HealAction healAction;
        private void Start()
        {
            healAction.OnRecastProgress += SetGauge;
            healAction.OnActionSkill += SetCount;
        }

        private void OnDestroy()
        {
            healAction.OnRecastProgress -= SetGauge;
            healAction.OnActionSkill -= SetCount;
        }
    }

}

