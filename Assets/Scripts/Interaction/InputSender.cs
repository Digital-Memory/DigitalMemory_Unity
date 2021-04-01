using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class InputSender : MonoBehaviour, IInputSender
{
    [OnValueChanged("OnChangeInputReference")]
    public InputObject input;
    [ShowIf("behaviourObjectIsCorrect")]
    [ShowAssetPreview(128, 128)]
    public GameObject inputObject;
    [HideInInspector] public bool behaviourObjectIsCorrect { get => ObjectsMatchBehaviours(); }

    public event Action OnSendInput;
    protected void OnChangeInputReference()
    {
        if (input != null)
        {
            inputObject = input.gameObject;
        }
    }

    private bool ObjectsMatchBehaviours()
    {
        return (input != null && inputObject != null && input.gameObject == inputObject);
    }


    protected virtual void CallOnSendInputEvents(float value)
    {
        OnSendInput?.Invoke();
    }
}
