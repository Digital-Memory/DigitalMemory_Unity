using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(ChangingOverTimeObject))]
public class ImpulseSenderOnFinishedAnimating : InputSender
{
    ChangingOverTimeObject changingOverTimeObject;
    [SerializeField] private float delay = 0f;

    [SerializeField] private InputType typeToSend;

    [SerializeField] [ShowIf("TypeIsBool")] private bool boolValueToSend;
    [SerializeField] [ShowIf("TypeIsFloat")] [Range(0f, 1f)] private float floatValueToSend;

    [HideInInspector] public bool TypeIsBool { get => typeToSend == InputType.Bool; }
    [HideInInspector] public bool TypeIsFloat { get => typeToSend == InputType.Float; }


    void OnEnable()
    {
        changingOverTimeObject = GetComponent<ChangingOverTimeObject>();
        if (changingOverTimeObject != null)
        {
            changingOverTimeObject.OnFinishedAnimatingEvent += OnFinishedAnimating;
        }
    }

    void OnDisable()
    {
        if (changingOverTimeObject != null)
        {
            changingOverTimeObject.OnFinishedAnimatingEvent -= OnFinishedAnimating;
        }
    }

    private void OnFinishedAnimating()
    {
        if (!input)
            return;

        if (delay > 0)
        {
            StopAllCoroutines();
            StartCoroutine(SendImpulseDelayedRoutine(delay));
        }
        else
            SendImpulseNow();
    }

    private IEnumerator SendImpulseDelayedRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        SendImpulseNow();
    }

    public void SendImpulseNow()
    {
        input.Try(typeToSend, boolValueToSend, floatValueToSend);
    }
}
