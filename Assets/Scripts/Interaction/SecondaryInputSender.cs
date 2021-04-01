using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Linq;
using System;
using UnityEngine.Assertions.Must;

public interface IInputSender
{
    event System.Action OnSendInput;
}
public class SecondaryInputSender : MonoBehaviour
{
    [InfoBox("Use this component to send Input to other objects without blockig input if they dont meet all conditions.", EInfoBoxType.Normal)]

    [OnValueChanged("OnChangeInputReference")]
    public List<InputObject> input;
    [ShowIf("behaviourObjectIsCorrect")]
    [ShowAssetPreview(128, 128)]
    public GameObject[] inputObject;
    [HideInInspector] public bool behaviourObjectIsCorrect { get => ObjectsMatchBehaviours(); }

    public event Action OnSendInput;

    private bool ObjectsMatchBehaviours()
    {
        for (int i = 0; i < input.Count; i++)
        {
            if (inputObject.Length > i)
            {
                InputObject component = input[i];
                GameObject gameObject = inputObject[i];

                if (component != null || gameObject != null || component.gameObject != gameObject)
                    return false;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    private void OnChangeInputReference()
    {
        inputObject = new GameObject[input.Count];

        for (int i = 0; i < input.Count; i++)
        {
            if (input[i] != null)
                inputObject[i] = input[i].gameObject;
        }
    }


    private void OnEnable()
    {
        InputSender sender = GetComponent<InputSender>();

        if (sender == null)
            Debug.LogError("No IInputSender found on " + gameObject.name);
        else
            sender.OnSendInput += SendInput;
    }

    private void OnDisable()
    {
        IInputSender sender = GetComponent<IInputSender>();

        if (sender == null)
            Debug.LogError("No IInputSender found on " + gameObject.name);
        else
            sender.OnSendInput -= SendInput;
    }

    private void SendInput()
    {
        foreach (InputObject obj in input)
        {
            if (obj != null)
                obj.Try();
        }
    }
}
