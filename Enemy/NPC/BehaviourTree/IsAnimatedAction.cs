using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "IsAnimated", story: "[Animation] is Active?", category: "Action", id: "d133e8caa269fc453d5f951bd82b7dd9")]
public partial class IsAnimatedAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> Animation;

    protected override Status OnStart()
    {
        Animation.Value = true;
        return Status.Running;
    }


    protected override Status OnUpdate()
    {
        if (!Animation.Value) return Status.Success;

        return Status.Running;

    }
}

