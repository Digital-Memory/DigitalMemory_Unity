using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public interface IInputSender
{
    event System.Action OnSendInput;
}
public class SecondaryInputSender : MonoBehaviour
{
    [InfoBox("Use this component to send Input to multiple objects. Note though that it only sends the input if the conditions on the original object are met or if it's NULL.", EInfoBoxType.Normal)]
    public List<InputObject> input;

    public event Action OnSendInput;


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
