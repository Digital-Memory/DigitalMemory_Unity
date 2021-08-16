using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChangingOverTimeObject))]
public class ConditionListenerChangeOverTimeObject : ConditionListenerBehaviour
{
    ChangingOverTimeObject changingObject;
    float time;

    private void OnEnable()
    {
        changingObject = GetComponent<ChangingOverTimeObject>();

        if (changingObject != null)
        {
            changingObject.OnTimeChangeEvent += OnTimeChangeEvent;
            time = changingObject.Time;

        }
    }

    private void OnDisable()
    {
        if (changingObject != null)
        {
            changingObject.OnTimeChangeEvent -= OnTimeChangeEvent;
        }
    }

    private void OnTimeChangeEvent(float time)
    {
        this.time = time;
    }

    public override bool SupportsFloat()
    {
        return true;
    }

    public override float GetFloat()
    {
        return time;
    }
}
