using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class FloatLimiter : ConditionedObject
{
    public bool active = true;
    [MinMaxSlider(0f, 1f)]
    public Vector2 minMaxValue;

    internal bool IsInside(float newValue)
    {
        if (IsActive() && newValue > minMaxValue.x && newValue < minMaxValue.y)
            return true;

        return false;
   
    }

    private bool IsActive()
    {
        return active && CheckAllConditionsForTrue();
    }
}
