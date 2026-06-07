using System;
using Unity.Behavior;
using UnityEngine;
using MazeGame;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Find  a Target", story: "[Self] Find a [Target] in range [SearchRange]", category: "Conditions", id: "d0afc2e4e9493d7df5ebfe8e1e6cae9f")]
public partial class FindATargetCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    [SerializeReference] public BlackboardVariable<float> SearchRange;

    private const float viewAngle = 120.0f;    //視野角
    private Animator animator;


    public override bool IsTrue()
    {
        if (animator == null && Self.Value != null)
        {
            animator = Self.Value.GetComponent<Animator>();
        }

        // 安全対策：SearchRangeがセットされていない場合も弾く
        if (animator == null || Self.Value == null || Target.Value == null || SearchRange == null) return false;

        if (SearchTarget())
        {
            Debug.Log("Find A Target");
            return true;
        }
        else
        {
            return false;

        }

    }

    private bool SearchTarget()
    {

        // プレイヤーが透明化中なら見失ってる
        if(!Target.Value.gameObject.CompareTag(MazeGameConstants.PlayerConstants.Tag)) return false;

        // 軽い処理で条件を絞る
        float distance = Vector3.Distance(Target.Value.transform.position, Self.Value.transform.position);

        if (distance > SearchRange.Value) return false;


        Transform headTransform = animator.GetBoneTransform(HumanBodyBones.Head);
        if (headTransform == null) return false;

        // 自分とプレイヤーの間に障害物があるかを確認する
        int layerMask = LayerMask.GetMask("Obstacle", "Gimic");
        if (Physics.Linecast(headTransform.position, Target.Value.transform.position, layerMask)) return false;

        //ターゲットが自身の視野角以内に存在するか調べる
        //自分→ターゲットの向きを取得する
        Vector3 directTarget = Target.Value.transform.position - headTransform.position;
        directTarget.Normalize();
        //自身の向きを取得する
        Vector3 forward = headTransform.forward;
        forward.Normalize();
        //内積を取得(値はCosθ)
        float dotCos = Vector3.Dot(forward, directTarget);
        dotCos = Mathf.Clamp(dotCos, -1f, 1f);
        float angle = Mathf.Acos(dotCos) * Mathf.Rad2Deg;
        float normalizedDist = Mathf.Clamp01(distance / SearchRange.Value);
        // 近ければ広い視野(180度)、遠ければ狭い視野(120度)にする
        float dynamicViewAngle = Mathf.Lerp(180f, viewAngle, normalizedDist);

        // 判定
        return angle <= (dynamicViewAngle / 2f);
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}