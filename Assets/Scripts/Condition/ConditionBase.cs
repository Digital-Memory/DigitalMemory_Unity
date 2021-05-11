using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionBase : MonoBehaviour
{
    [Button]
    public virtual bool IsMet()
    {
        return false;
    }
}
