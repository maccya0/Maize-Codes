using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Change Agent Speed", story: "Change [Agent] Speed To [NewSpeed]", category: "Action", id: "7be3e04316ac39fa1e09b3eb6ad09d27")]
public partial class ChangeAgentSpeedAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<float> NewSpeed;
    private NavMeshAgent agent;

    protected override Status OnStart()
    {
        agent = Agent.Value.GetComponent<NavMeshAgent>();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        agent.speed = NewSpeed.Value;

        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

