using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionedObject : InputObject
{
    [SerializeField]
    private List<ConditionBase> conditions;

    [SerializeField]
    private bool sendsFollowupInputWhenAllConditionsAreMet = false;
    [SerializeField]
    [ShowIf("sendsFollowupInputWhenAllConditionsAreMet")]
    private InputObject[] inputObjects;
    [SerializeField]
    [ShowIf("sendsFollowupInputWhenAllConditionsAreMet")]
    private InputType typeToSend;

    [SerializeField] [ShowIf(EConditionOperator.And, "sendsFollowupInputWhenAllConditionsAreMet", "TypeIsBool")] private bool boolValueToSend;
    [SerializeField] [ShowIf(EConditionOperator.And, "sendsFollowupInputWhenAllConditionsAreMet", "TypeIsFloat")] [Range(0f, 1f)] private float floatValueToSend;

    private const float MAX_DISTANCE_TO_COUNT_AS_EQUAL = 0.1f;

    [HideInInspector] public bool TypeIsBool { get => typeToSend == InputType.Bool; }
    [HideInInspector] public bool TypeIsFloat { get => typeToSend == InputType.Float; }

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
                {
                    //Debug.Log($"condition {condition.name} is FALSE");
                    return false;
                }
            }
        }

        if (sendsFollowupInputWhenAllConditionsAreMet)
            Try(inputObjects, typeToSend, boolValue: boolValueToSend, floatValue: floatValueToSend);

        //Debug.Log("All conditions are TRUE");

        return true;
    }
}
