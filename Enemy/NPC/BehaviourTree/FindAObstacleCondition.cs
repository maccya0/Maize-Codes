using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Find a Obstacle", story: "[Self] is Find [ObstacleTag] Object", category: "Conditions", id: "5d97108d9535c16c9139d706e65459e4")]
public partial class FindAObstacleCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<List<string>> ObstacleTag;
    private float searchRange = 3.0f;
    private float rayDuration = 0.1f;

    public override bool IsTrue()
    {
        return SearchTarget();
    }
    private bool SearchTarget()
    {
        Vector3 rayPos = Self.Value.transform.position;
        Ray ray = new Ray(rayPos, Self.Value.transform.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * searchRange, Color.cyan, rayDuration, false);
        if (Physics.Raycast(ray, out hit, searchRange))
        {
            foreach (String tag in ObstacleTag.Value)
            {
                if (hit.collider.gameObject.tag == tag)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
