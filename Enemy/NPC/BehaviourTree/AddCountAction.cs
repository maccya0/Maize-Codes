using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AddCount", story: "[Variable] Add [Count]", category: "Action/Blackboard", id: "1ca71a7903327a82f5602f3e90e702e3")]
public partial class AddCountAction : Action
{
    [SerializeReference] public BlackboardVariable<int> Variable;
    [SerializeReference] public BlackboardVariable<int> Count;

    protected override Status OnStart()
    {
        int current = Variable.Value;
        current += Count;
        Variable.Value = current;
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

