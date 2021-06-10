using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType
{
    Impulse,
    Bool,
    Float,
}

[RequireComponent(typeof(Attacher))]
public class AttacherSender : InputSender
{
    [SerializeField] InputType typeToSend;
    [SerializeField] [ShowIf("TypeIsImpulse")] bool sendOnlyIfAttached;
    [HideInInspector] public bool TypeIsImpulse => (typeToSend == InputType.Impulse);

    Attacher attacher;
    private void Start()
    {
        attacher = GetComponent<Attacher>();

        if (attacher == null)
        {
            Debug.LogError("AttacherSender added to GameObject without Attacher. Destroyed Automatically.");
            Destroy(this);
        }
        else
        {
            attacher.OnChangeAttached += OnChangeAttached;
        }
    }

    private void OnDisable()
    {
        if (attacher != null)
            attacher.OnChangeAttached -= OnChangeAttached;
    }

    private void OnChangeAttached(bool isAttached, string attachment)
    {
        Debug.Log($"Attacher: {gameObject.name} send attach input: {isAttached}");

        switch (typeToSend)
        {
            case InputType.Impulse:
                if (isAttached || !sendOnlyIfAttached)
                    if (input.Try())
                    {
                        SendSecondaryInput(InputType.Impulse);
                    }
                return;

            case InputType.Bool:
                if (input.Try(isAttached))
                {
                    SendSecondaryInput(InputType.Bool, boolValue: isAttached);
                }
                return;

            default:
                Debug.LogError("AttacherSender can only send Impulse or Bool.");
                return;
        }
    }
}
