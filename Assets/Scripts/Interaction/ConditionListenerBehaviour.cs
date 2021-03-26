using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionListenerBehaviour : MonoBehaviour
{
    public virtual bool GetBool()
    {
        Debug.LogError("Checked for bool value on an object that does not support bool value checks...");
        return true;
    }

    public virtual float GetFloat()
    {
        return float.MinValue;
    }
}