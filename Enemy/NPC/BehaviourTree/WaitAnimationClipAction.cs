using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "WaitAnimationClip", story: "[Self] [AnimationClip] is Runnnunng", category: "Action", id: "d5bfb350150f64b391402289cbe51ebe")]
public partial class WaitAnimationClipAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<string> AnimationClip;
    private Animator animator;
    private bool isAnimated;

    protected override Status OnStart()
    {
        animator = Self.Value.GetComponent<Animator>();
        if(animator == null) return Status.Failure;
        isAnimated = false;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        var info = animator.GetCurrentAnimatorStateInfo(0);
        var nextInfo = animator.GetNextAnimatorStateInfo(0);


        // パターンA: まだ対象のアニメーションを再生中だが、終わりかけている場合
        if (info.IsName(AnimationClip.Value))
        {
            isAnimated = true;
            if (info.normalizedTime >= 0.95f)
            {
                isAnimated = false;
                return Status.Success;
            }
        }

        // パターンB: すでに別のアニメーション（Idleなど）に切り替わってしまった場合
        // 「直前のステートが対象のアニメーションだったか」は直接取れないため、
        // 「対象ではないステートにいる」＝「終わった」とみなすロジック
        if (!info.IsName(AnimationClip.Value) && !nextInfo.IsName(AnimationClip.Value) && isAnimated)
        {
            isAnimated = false;
            return Status.Success;
        }
        return Status.Running;
    }
}

