using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "distance between Self and Target", story: "distance between [Self] and [Target] Lower than [Val]", category: "Conditions", id: "625dead898294f97c2472b9a9147dd6e")]
public partial class DistanceBetweenSelfAndTargetCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> Val;

    public override bool IsTrue()
    {
        if (Self == null || Self.Value == null || Target == null || Target.Value == null)
        {
            return false;
        }
        return JudgeDist();
    }
    //Žw’è‚³‚ê‚½‹——£ˆÈ“à‚©‚Ç‚¤‚©‚ð•Ô‚·
    private bool JudgeDist()
    {
        if (Val > Vector3.Distance(Self.Value.transform.position, Target.Value.transform.position))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
