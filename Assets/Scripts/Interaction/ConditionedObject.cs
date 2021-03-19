using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionedObject : InputObject
{
    [SerializeField]
    private List<Condition> conditions;

    protected virtual void OnEnable()
    {
        conditions = new List<Condition>(GetComponents<Condition>());
    }

    public override bool Try(float progress)
    {
        return CheckAllConditionsForTrue();
    }

    private bool CheckAllConditionsForTrue()
    {
        foreach (Condition condition in conditions)
        {
            if (condition.behaviour != null)
            {
                if (!condition.IsMet())
                    return false;
            }
        }

        return true;
    }

    protected virtual void OnDrawGizmos()
    {
        foreach (Condition condition in conditions)
        {
            if (condition.behaviour != null)
            {
                Gizmos.color = condition.IsMet()?Color.green:Color.red;
                Gizmos.DrawLine(transform.position, condition.behaviour.transform.position);
            }
        }
    }
}
