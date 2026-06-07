using UnityEngine;

namespace MazeGame
{
    public class TransparentGauge : SkillGauge
    {
        [SerializeField] private TransparentAction transparentAction;
        private void Start()
        {
            transparentAction.OnRecastProgress += SetGauge;
            transparentAction.OnActionSkill += SetCount;
        }

        private void OnDestroy()
        {
            transparentAction.OnRecastProgress -= SetGauge;
            transparentAction.OnActionSkill -= SetCount;
        }

    }

}
