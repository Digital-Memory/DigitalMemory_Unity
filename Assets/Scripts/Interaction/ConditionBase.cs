using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionBase : MonoBehaviour
{
    public virtual bool IsMet()
    {
        return false;
    }
}
