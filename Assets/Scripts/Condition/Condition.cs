using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[System.Serializable]
public class Condition : ConditionBase
{
    [OnValueChanged("OnChangeBehaviourReference")]
    [ValidateInput("BehaviourObjectIsCorrect", "Select a correct behaviour!")]
    public ConditionListenerBehaviour behaviour;
    [ShowIf("behaviourObjectIsCorrect")]
    [ShowAssetPreview(128, 128)]
    public GameObject behaviourObject;
    [OnValueChanged("OnTypeChangedCallback")]
    public ConditionType type;

    [ShowIf("TypeIsBool")] public bool MustBeTrue;
    [ShowIf("TypeIsFloat")] public FloatCompare FloatIs;
    [ShowIf("TypeIsFloat")] [Range(0f, 1f)] public float toCompareWith;

    private const float MAX_DISTANCE_TO_COUNT_AS_EQUAL = 0.1f;

    [HideInInspector] public bool TypeIsBool { get => type == ConditionType.BOOL; }
    [HideInInspector] public bool TypeIsFloat { get => type == ConditionType.FLOAT; }
    [HideInInspector] public bool behaviourObjectIsCorrect { get => BehaviourObjectIsCorrect(); }

    private bool BehaviourObjectIsCorrect()
    {
        return (behaviour != null && behaviourObject != null && behaviourObject == behaviour.gameObject);
    }

    private DropdownList<ConditionListenerBehaviour> GetConditionListenerBehaviours()
    {
        return DropdownMonobehaviourList<ConditionListenerBehaviour>.FromObjectsOfType(FindObjectsOfType<ConditionListenerBehaviour>());
    }

    private void OnChangeBehaviourReference()
    {
        behaviourObject = behaviour.gameObject;
    }

    private void OnTypeChangedCallback()
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


#if UNITY_EDITOR
    [Button]
    protected void FixBehaviour()
    {
        behaviour = behaviour;
    }

    private void Reset()
    {
        ConditionedObject co = GetComponent<ConditionedObject>();
        if (co == null)
            Debug.LogError("Conditions can only be added to Conditioned Objects.");
        else
            co.TryLoadConditions();
    }
#endif

    public override bool IsMet()
    {
        if (!behaviour) Debug.LogError($"Behaviour on condition {name} is NULL");

        switch (type)
        {
            case ConditionType.BOOL:
                return (behaviour.GetBool() == MustBeTrue);

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

        Debug.Log($"Conditon {name} is not met.");

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
