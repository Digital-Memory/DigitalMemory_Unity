using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

[RequireComponent(typeof(FloatSender))]
public class ImpulseSenderOnFloatReach : InputSender
{
    FloatSender floatSender;

    private const float MAX_DISTANCE_TO_COUNT_AS_EQUAL = 0.1f;

    public FloatCompare FloatIs;
    [Range(0f, 1f)] public float toCompareWith;

    void OnEnable()
    {
        floatSender = GetComponent<FloatSender>();
        if (floatSender != null)
        {
            floatSender.OnSendInputValue += OnSendFloatValue;
        }
    }

    void OnDisable()
    {
        if (floatSender != null)
        {
            floatSender.OnSendInputValue -= OnSendFloatValue;
        }
    }

    private void OnSendFloatValue(float value)
    {
        if (CheckConditions(value))
        {
            bool ReturnedTrue = input != null && input.Try();
            if (ReturnedTrue)
            {
                CallOnSendInputEvents(0f);
            }

            Debug.Log($"Send Impulse from {gameObject.name} : returned {ReturnedTrue} ");
        }
    }

    private bool CheckConditions(float value)
    {
        switch (FloatIs)
        {
            case FloatCompare.EQUALS:
                return (Mathf.Abs(value - toCompareWith) < MAX_DISTANCE_TO_COUNT_AS_EQUAL);
            case FloatCompare.GREATER:
                return (value > toCompareWith);
            case FloatCompare.SMALLER:
                return (value < toCompareWith);
        }

        return false;
    }
}
