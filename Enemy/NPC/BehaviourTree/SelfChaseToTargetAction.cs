using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Self Chase to Target", story: "[Self] Chase to [Target]", category: "Action", id: "154f910710dc0563aa9c5736cbe2d2d0")]
public partial class SelfChaseToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    private float Speed=3.0f;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Vector3 direction = Target.Value.transform.position-Self.Value.transform.position;
        direction.y = 0;
        direction.Normalize();
        Self.Value.transform.forward = direction;
        Self.Value.transform.Translate(Self.Value.transform.forward *Speed *Time.deltaTime);

        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

