using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[ExecuteAlways]
[System.Serializable]
public class Condition : MonoBehaviour
{
    public ConditionBehaviour behaviour;
    public ConditionType type;

    [ShowIf("TypeIsBool")] public bool MustBeTrue;
    [ShowIf("TypeIsFloat")] public FloatCompare FloatIs;
    [ShowIf("TypeIsFloat")] public float toCompareWith;

    [HideInInspector] public bool TypeIsBool { get => type == ConditionType.BOOL; }
    [HideInInspector] public bool TypeIsFloat { get => type == ConditionType.FLOAT; }

    void OnEable()
    {
        ConditionedObject connectTo = GetComponent<ConditionedObject>();
        if (connectTo != null)
            connectTo.conditions.Add(this);
    }

    void OnDisable()
    {
        ConditionedObject connectTo = GetComponent<ConditionedObject>();
        if (connectTo != null)
            connectTo.conditions.Remove(this);
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
