using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class FloatLimiter : ConditionedObject
{
    public bool active = true;
    public float minValue, maxValue;

    internal bool IsInside(float newValue)
    {
        if (IsActive() && newValue > minValue && newValue < maxValue)
            return true;

        return false;
   
    }

    private bool IsActive()
    {
        return active && CheckAllConditionsForTrue();
    }
}
