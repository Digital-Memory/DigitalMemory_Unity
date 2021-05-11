using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionListenerBlockedGear : ConditionListenerBehaviour
{
    BlockedGear gear;
    public bool IsFree;

    void OnEnable()
    {
        gear = GetComponent<BlockedGear>();
        if (gear != null)
        {
            gear.OnFreeGear += OnFreeGear;
        }
    }

    void OnDisable()
    {
        if (gear != null)
        {
            gear.OnFreeGear -= OnFreeGear;
        }
    }

    public override bool GetBool()
    {
        return IsFree;
    }

    private void OnFreeGear()
    {
        IsFree = true;
    }
}
