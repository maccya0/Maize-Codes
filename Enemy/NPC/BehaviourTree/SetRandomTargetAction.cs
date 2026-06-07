using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetRandomTarget", story: "[Target] is Selected From [PatrolPoint]", category: "Action", id: "7af4e99988f87ed20284c878bd5d8f38")]
public partial class SetRandomTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<List<GameObject>> PatrolPoint;

    protected override Status OnStart()
    {
        if (PatrolPoint.Value.Count > 0)
        {
            // ƒ‰ƒ“ƒ_ƒ€‚Éˆê‚Â‘I‚Ô
            int randomIndex = UnityEngine.Random.Range(0, PatrolPoint.Value.Count);
            // ‚»‚ÌÀ•W‚ğBlackboard‚É‘‚«‚Ş
            Target.Value = PatrolPoint.Value[randomIndex];
            return Status.Success;
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

