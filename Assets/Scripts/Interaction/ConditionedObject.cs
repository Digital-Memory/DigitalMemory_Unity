using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionedObject : InputObject
{
    public List<Condition> conditions = new List<Condition>();

    public override bool Try(float progress)
    {
        return CheckAllConditionsForTrue();
    }

    private bool CheckAllConditionsForTrue()
    {
        return true;
    }

    protected virtual void OnDrawGizmos()
    {
        foreach (Condition condition in conditions)
        {
            if (condition.behaviour != null)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(transform.position, condition.behaviour.transform.position);
            }
        }
    }
}
