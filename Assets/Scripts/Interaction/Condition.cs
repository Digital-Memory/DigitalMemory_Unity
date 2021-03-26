using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

[System.Serializable]
public class Condition : MonoBehaviour
{
    [OnValueChanged("OnChangeBehaviourReference")]
    public ConditionListenerBehaviour behaviour;
    [ShowIf("behaviourObjectIsCorrect")]
    [ShowAssetPreview(128, 128)]
    public GameObject behaviourObject;
    public ConditionType type;

    [ShowIf("TypeIsBool")] public bool MustBeTrue;
    [ShowIf("TypeIsFloat")] public FloatCompare FloatIs;
    [ShowIf("TypeIsFloat")] public float toCompareWith;

    private const float MAX_DISTANCE_TO_COUNT_AS_EQUAL = 0.1f;

    [HideInInspector] public bool TypeIsBool { get => type == ConditionType.BOOL; }
    [HideInInspector] public bool TypeIsFloat { get => type == ConditionType.FLOAT; }
    [HideInInspector] public bool behaviourObjectIsCorrect { get => (behaviourObject != null && behaviourObject == behaviour.gameObject); }

    private void OnChangeBehaviourReference()
    {
        behaviourObject = behaviour.gameObject;
    }

    public bool IsMet()
    {
        switch (type)
        {
            case ConditionType.BOOL:
                return (behaviour.GetBool() == MustBeTrue);
                break;

            case ConditionType.FLOAT:
                switch (FloatIs)
                {
                    case FloatCompare.EQUALS:
                        return (Mathf.Abs(behaviour.GetFloat() - toCompareWith) < MAX_DISTANCE_TO_COUNT_AS_EQUAL);
                    case FloatCompare.GREATER:
                        return (behaviour.GetFloat() > toCompareWith);
                    case FloatCompare.SMALLER:
                        return (behaviour.GetFloat() < toCompareWith);
                }
                break;
        }

        return false;
    }
}

public enum ConditionType
{
    BOOL,
    FLOAT
}

public enum FloatCompare
{
    GREATER,
    SMALLER,
    EQUALS,
}
