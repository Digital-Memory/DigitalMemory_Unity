using NaughtyAttributes;
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
        } else
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
                    input.Try();
                    return;

            case InputType.Bool:
                input.Try(isAttached);
                return;

            default:
                Debug.LogError("AttacherSender can only send Impulse or Bool.");
                return;
        }
    }
}
