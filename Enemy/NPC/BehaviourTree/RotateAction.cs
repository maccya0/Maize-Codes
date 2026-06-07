using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Rotate", story: "[Self] Roted [Val]", category: "Action", id: "2dc15061708f3788b937468299ca5cbb")]
public partial class RotateAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<float> Val;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Self.Value.transform.Rotate(new Vector3(0, Val * Time.deltaTime, 0));
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

