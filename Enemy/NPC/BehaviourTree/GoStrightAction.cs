using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GoStright", story: "[Target] go Stright", category: "Action", id: "572d9b5f0c70d6aa7272dadea00ef595")]
public partial class GoStrightAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    private float Speed = 5;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Self.Value.transform.Translate(Vector3.forward* Speed * Time.deltaTime);
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

