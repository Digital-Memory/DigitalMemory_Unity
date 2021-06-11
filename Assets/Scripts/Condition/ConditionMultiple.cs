using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;
using System.Linq;

[System.Serializable]
public class ConditionMultiple : ConditionBase
{
    public ConditionListenerBehaviour[] behaviours;
    [OnValueChanged("OnTypeChangedCallback")]
    public ConditionType type;

    [ShowIf("TypeIsBool")] public bool MustBeTrue;
    [ShowIf("TypeIsFloat")] public FloatCompare FloatIs;
    [ShowIf("TypeIsFloat")] [Range(0f, 1f)] public float toCompareWith;

    private const float MAX_DISTANCE_TO_COUNT_AS_EQUAL = 0.1f;

    [HideInInspector] public bool TypeIsBool { get => type == ConditionType.BOOL; }
    [HideInInspector] public bool TypeIsFloat { get => type == ConditionType.FLOAT; }

    private void OnTypeChangedCallback()
    {
        foreach (ConditionListenerAttacher behaviour in behaviours)
        {
            if (behaviour != null)
            {
                if (type == ConditionType.BOOL && !behaviour.SupportsBool())
                {
                    type = ConditionType.FLOAT;
                    Debug.LogError("This behaviour does not support bool conditions.");
                }
                if (type == ConditionType.FLOAT && !behaviour.SupportsFloat())
                {
                    type = ConditionType.BOOL;
                    Debug.LogError("This behaviour does not support float conditions.");
                }
            }
        }
    }

    public override bool IsMet()
    {
        switch (type)
        {
            case ConditionType.BOOL:

                int i = 0;
                foreach (ConditionListenerAttacher behaviour in behaviours)
                {
                    if (behaviour != null)
                    {
                        if (CeckBehaviourConditionToBeTrue(behaviour))
                        {
                            DebugDraw.Cross(behaviour.gameObject.transform.position + Vector3.up * Game.Settings.CurrentZoomLevel, Color.red, 2 * Game.Settings.CurrentZoomLevel);
                        }
                        else
                        {
                            i++;
                            DebugDraw.Circle(behaviour.gameObject.transform.position + Vector3.up * Game.Settings.CurrentZoomLevel, Color.green, 1 * Game.Settings.CurrentZoomLevel);
                        }
                    }
                }
                if (i == behaviours.Length)
                {
                    //Debug.Log($"Conditon check success ({behaviours.Length}) on {gameObject.name}");
                    return true;
                }
                else
                {
                    //Debug.Log($"Conditon check failed ({i} / {behaviours.Length}) on {gameObject.name}");
                    return false;
                }
        }

        return true;
    }

    private bool CeckBehaviourConditionToBeTrue(ConditionListenerAttacher behaviour)
    {
        if (behaviour == null)
        {
            Debug.LogWarning("Behaviour on ConditionMultiple was null, returned true for that one");
            return true;
        }

        if (type == ConditionType.FLOAT)
        {
            switch (FloatIs)
            {
                case FloatCompare.EQUALS:
                    if (Mathf.Abs(behaviour.GetFloat() - toCompareWith) > MAX_DISTANCE_TO_COUNT_AS_EQUAL)
                        return false;
                    break;

                case FloatCompare.GREATER:
                    if (behaviour.GetFloat() < toCompareWith)
                        return false;
                    break;

                case FloatCompare.SMALLER:

                    if (behaviour.GetFloat() > toCompareWith)
                        return false;
                    break;
            }
        }
        else if (type == ConditionType.BOOL)
        {
            return behaviour.GetBool() != MustBeTrue;
        }

        return false;
    }

#if UNITY_EDITOR
    private void Reset()
    {
        ConditionedObject co = GetComponent<ConditionedObject>();
        if (co == null)
            Debug.LogError("Conditions can only be added to Conditioned Objects.");
        else
            co.TryLoadConditions();
    }
#endif

}
