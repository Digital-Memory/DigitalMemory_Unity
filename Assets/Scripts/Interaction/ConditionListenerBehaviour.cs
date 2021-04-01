using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionListenerBehaviour : MonoBehaviour
{
    public virtual bool GetBool()
    {
        Debug.LogError("Checked for bool value on an object that does not support bool value checks... (" + gameObject.name + ")");
        return true;
    }

    public virtual float GetFloat()
    {
        Debug.LogError("Checked for float value on an object that does not support float value checks... (" + gameObject.name + ")");
        return float.MinValue;
    }

    public virtual bool SupportsBool()
    {
        return false;
    }

    public virtual bool SupportsFloat()
    {
        return false;
    }
}
