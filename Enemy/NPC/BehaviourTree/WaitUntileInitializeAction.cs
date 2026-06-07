using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Wait Untile Initialize", story: "[Flag] Is Compleate", category: "Action", id: "ebbaea00aa3d4289d953aa80241fe458")]
public partial class WaitUntileInitializeAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> Flag;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(Flag.Value)
        {
            return Status.Success;
        }
        else
        {
            return Status.Failure;
        }
    }

    protected override void OnEnd()
    {
    }
}

