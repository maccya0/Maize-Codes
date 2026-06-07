using UnityEngine;

namespace MazeGame
{
    public class DestroyObjGauge : SkillGauge
    {
        [SerializeField] private DestroyObjAction destroyObjAction;
        private void Start()
        {
            destroyObjAction.OnRecastProgress += SetGauge;
            destroyObjAction.OnActionSkill += SetCount;
        }

        private void OnDestroy()
        {
            destroyObjAction.OnRecastProgress -= SetGauge;
            destroyObjAction.OnActionSkill -= SetCount;
        }
    }

}

