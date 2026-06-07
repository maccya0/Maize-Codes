using UnityEngine;
using System.Collections.Generic;
using Unity.Behavior;

public class CollectMazeInfo : MonoBehaviour
{
    private BehaviorGraphAgent agent;

    public void Init()
    {
        agent = GetComponent<BehaviorGraphAgent>();
    }

    public void RegisterPlayer(GameObject target)
    {
        if (agent == null) return;
        agent.SetVariableValue("Target", target);
    }

    public bool IsGeneratedAgent()
    {
        agent = GetComponent<BehaviorGraphAgent>();
        return agent != null;

    }

    public void ComplateInitialize()
    {
        if (agent == null) return;
        const bool InitializeComplate = true;
        agent.SetVariableValue("IsInitialize", InitializeComplate);
    }


    public void RegisterPatrolPoint(GameObject target)
    {
        if (agent == null) return;

        if (agent.GetVariable("Patrol points", out BlackboardVariable<List<GameObject>> bbVariable))
        {
            List<GameObject> currentPoints = bbVariable.Value;

            if (currentPoints == null)
            {
                currentPoints = new List<GameObject>();
                bbVariable.Value = currentPoints;
            }

            if (!currentPoints.Contains(target))
            {
                currentPoints.Add(target);
            }

            agent.SetVariableValue("Patrol points", currentPoints);
        }
    }

    public void RemovePatrolPoint(GameObject target)
    {
        if (agent == null) return;

        if (agent.GetVariable("Patrol points", out BlackboardVariable<List<GameObject>> bbVariable))
        {
            List<GameObject> currentPoints = bbVariable.Value;

            if (currentPoints != null)
            {
                if (currentPoints.Contains(target))
                {
                    currentPoints.Remove(target);
                }

                agent.SetVariableValue("Patrol points", currentPoints);
            }
        }
    }

    public void InformPlayerPos(Vector3 pos)
    {
        agent.SetVariableValue("LastKnownPlayerPos", pos);
        agent.SetVariableValue("IsPlayerDetected", true);
    }
}