using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

[System.Serializable]
public class Condition : MonoBehaviour
{
    [OnValueChanged("OnChangeBehaviourReference")]
    public ConditionBehaviour behaviour;
    [ShowIf("behaviourObjectIsCorrect")]
    [ShowAssetPreview(128, 128)]
    public GameObject behaviourObject;
    public ConditionType type;

    [ShowIf("TypeIsBool")] public bool MustBeTrue;
    [ShowIf("TypeIsFloat")] public FloatCompare FloatIs;
    [ShowIf("TypeIsFloat")] public float toCompareWith;

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
