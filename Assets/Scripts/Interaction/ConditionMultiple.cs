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

                foreach (ConditionListenerAttacher behaviour in behaviours)
                {
                    if (behaviour != null)
                    {
                        if (behaviour.GetBool() != MustBeTrue)
                        {
                            DebugDraw.Cross(behaviour.gameObject.transform.position + Vector3.up * Game.Settings.CurrentZoomLevel, Color.red, 2 * Game.Settings.CurrentZoomLevel);
                            return false;
                        } else
                        {
                            DebugDraw.Circle(behaviour.gameObject.transform.position + Vector3.up * Game.Settings.CurrentZoomLevel, Color.green, 1 * Game.Settings.CurrentZoomLevel);
                        }
                    }
                }
                break;



            case ConditionType.FLOAT:
                switch (FloatIs)
                {
                    case FloatCompare.EQUALS:

                        foreach (ConditionListenerAttacher behaviour in behaviours)
                        {
                            if (behaviour != null)
                            {
                                if (Mathf.Abs(behaviour.GetFloat() - toCompareWith) > MAX_DISTANCE_TO_COUNT_AS_EQUAL)
                                    return false;
                            }
                        }
                        break;

                    case FloatCompare.GREATER:
                        foreach (ConditionListenerAttacher behaviour in behaviours)
                        {
                            if (behaviour != null)
                            {
                                if (behaviour.GetFloat() < toCompareWith)
                                    return false;
                            }
                        }
                        break;

                    case FloatCompare.SMALLER:
                        foreach (ConditionListenerAttacher behaviour in behaviours)
                        {
                            if (behaviour != null)
                            {
                                if (behaviour.GetFloat() > toCompareWith)
                                    return false;
                            }
                        }
                        break;
                }
                break;
        }

        return true;
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
