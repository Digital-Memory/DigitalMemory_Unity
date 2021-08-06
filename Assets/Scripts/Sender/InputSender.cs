using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.PlayerLoop;

public interface IInputSender
{
    event System.Action OnSendInput;
}

#if UNITY_EDITOR

public enum InputSelectionType
{
    Dropdown,
    Reference,
}

#endif

public class InputSender : MonoBehaviour, IInputSender
{
#if UNITY_EDITOR

    float activeTime = 0;

    [InfoBox("Use dropdowns only for scene references.")]
    [SerializeField] protected InputSelectionType inputSelectionType;

    bool inputSelectionTypeIsDropdown => inputSelectionType == InputSelectionType.Dropdown;
    bool inputSelectionTypeIsReference => inputSelectionType == InputSelectionType.Reference;


    [ShowIf("inputSelectionTypeIsDropdown")]
    [OnValueChanged("OnChangeInputDropdown")]
    [Dropdown("CreateInputDropdown")]
    [SerializeField]
    protected InputObject inputDropown;

    protected void OnChangeInputDropdown()
    {
        input = inputDropown;
        inputObject = inputDropown.gameObject;
    }


    [ShowIf("inputSelectionTypeIsReference")]
#endif
    [OnValueChanged("OnChangeInputReference")]
    [ValidateInput("ObjectsMatchBehaviours", "Select a matching Input Objects, or ignore if you are using a SecondaryInputSender")]
    [SerializeField]
    protected InputObject input;
    protected void OnChangeInputReference()
    {
        if (input != null)
        {
            inputObject = input.gameObject;
        }
    }
    protected bool ObjectsMatchBehaviours()
    {
        return (input != null && inputObject != null && input.gameObject == inputObject);
    }

    //[ShowIf("behaviourObjectIsCorrect")]
    [ShowAssetPreview(128, 128)]
    [SerializeField]
    protected GameObject inputObject;
    [HideInInspector] public bool behaviourObjectIsCorrect { get => ObjectsMatchBehaviours(); }

    [SerializeField ] protected bool hasSecondaryInput;
    [SerializeField] [ShowIf("hasSecondaryInput")] protected InputObject[] secondary;



    protected DropdownList<InputObject> CreateInputDropdown()
    {
        return DropdownMonobehaviourList<InputObject>.FromObjectsOfType(FindObjectsOfType<InputObject>(), AddNullOption: true);
    }

    public event Action OnSendInput;

#if UNITY_EDITOR
    [Button]
    protected void FixInput()
    {
        input = input;
    }
#endif

    protected void SendSecondaryInput(InputType type, bool boolValue = true, float floatValue = 1f)
    {
#if UNITY_EDITOR
        activeTime = Time.time;
#endif

        if (!hasSecondaryInput)
            return;

        string str = "send " + type.ToString() + " to :";

        foreach (InputObject input in secondary)
        {
            switch(type)
            {
                case InputType.Impulse:
                    input.Try();
                    break;

                case InputType.Bool:
                    input.Try(boolValue);
                    break;

                case InputType.Float:
                    input.Try(floatValue);
                    break;
            }

            str += input.name + ", ";
        }

        Debug.Log(str);
    }

    protected virtual void CallOnSendInputEvents(float value)
    {
        OnSendInput?.Invoke();
    }

#if UNITY_EDITOR

    protected void OnDrawGizmos()
    {
        if (!input)
            return;

        Gizmos.color = activeTime + 1 > Time.time ? Color.yellow : Color.gray;

        Gizmos.DrawLine(transform.position, input.transform.position);

        if (!hasSecondaryInput)
            return;

        foreach (InputObject sec in secondary)
        {
            if (sec != null)
                Gizmos.DrawLine(transform.position, sec.transform.position);
        }
    }
#endif
}
