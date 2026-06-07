using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Get Player Pos", story: "Get [Player] [Pos]", category: "Action", id: "c2389811684f39b5981767f32b0bceca")]
public partial class GetPlayerPosAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<Vector3> Pos;

    protected override Status OnStart()
    {
        if (Player == null || Player.Value == null)
        {
            return Status.Failure;
        }

        GameObject player = Player.Value;
        Pos.Value = player.transform.position;
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

