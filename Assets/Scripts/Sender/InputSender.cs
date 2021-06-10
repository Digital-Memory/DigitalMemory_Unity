using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public interface IInputSender
{
    event System.Action OnSendInput;
}

public class InputSender : MonoBehaviour, IInputSender
{
    [OnValueChanged("OnChangeInputReference")]
    [ValidateInput("ObjectsMatchBehaviours", "Select a matching Input Objects, or ignore if you are using a SecondaryInputSender")]
    [Dropdown("CreateInputDropdown")]
    public InputObject input;
    [ShowIf("behaviourObjectIsCorrect")]
    [ShowAssetPreview(128, 128)]
    public GameObject inputObject;
    [HideInInspector] public bool behaviourObjectIsCorrect { get => ObjectsMatchBehaviours(); }

    [SerializeField ] protected bool hasSecondaryInput;
    [SerializeField] [ShowIf("hasSecondaryInput")] protected InputObject[] secondary;



    protected DropdownList<InputObject> CreateInputDropdown()
    {
        return DropdownMonobehaviourList<InputObject>.FromObjectsOfType(FindObjectsOfType<InputObject>(), AddNullOption: true);
    }

    public event Action OnSendInput;
    protected void OnChangeInputReference()
    {
        if (input != null)
        {
            inputObject = input.gameObject;
        }
    }

#if UNITY_EDITOR
    [Button]
    protected void FixInput()
    {
        input = input;
    }
#endif

    protected bool ObjectsMatchBehaviours()
    {
        return (input != null && inputObject != null && input.gameObject == inputObject);
    }

    protected void SendSecondaryInput(InputType type, bool boolValue = true, float floatValue = 1f)
    {
        if (!hasSecondaryInput)
            return;

        foreach (InputObject input in secondary)
        {
            switch(type)
            {
                case InputType.Impulse:
                    input.Try();
                    return;

                case InputType.Bool:
                    input.Try(boolValue);
                    return;

                case InputType.Float:
                    input.Try(floatValue);
                    return;
            }
        }
    }

    protected virtual void CallOnSendInputEvents(float value)
    {
        OnSendInput?.Invoke();
    }
}
