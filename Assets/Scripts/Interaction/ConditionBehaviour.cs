using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionBehaviour : MonoBehaviour
{
    public virtual bool GetBool()
    {
        Debug.LogError("Checked for bool value on an object that does not support bool value checks...");
        return true;
    }
}
