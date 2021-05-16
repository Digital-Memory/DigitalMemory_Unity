using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionedObject : InputObject
{
    [SerializeField]
    private List<ConditionBase> conditions;

    protected virtual void OnEnable()
    {
        TryLoadConditions();
    }

    public void TryLoadConditions()
    {
        if (GetComponents<ConditionedObject>().Length > 1)
        {
            Debug.LogWarning("Multiple conditioned objects found, please define conditions manually to decide which conditions should affect which object");
        }
        else
        {
            conditions = new List<ConditionBase>(GetComponents<ConditionBase>());
        }
    }

    public override bool Try()
    {
        return CheckAllConditionsForTrue();
    }

    public override bool Try(bool on)
    {
        return CheckAllConditionsForTrue();
    }

    public override bool Try(float progress)
    {
        return CheckAllConditionsForTrue();
    }

    protected bool CheckAllConditionsForTrue()
    {
        if (conditions != null)
        {
            foreach (ConditionBase condition in conditions)
            {
                if (!condition.IsMet())
                    return false;
            }
        }

        return true;
    }
}
