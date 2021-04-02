using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionListenerLever : ConditionListenerBehaviour
{
    FloatSender floatSender;
    public bool IsUp;

    void OnEnable()
    {
        floatSender = GetComponent<FloatSender>();
        if (floatSender != null)
        {
            floatSender.OnSendInputValue += AttacherChangeAttached;
        }
    }

    void OnDisable()
    {
        if (floatSender != null)
        {
            floatSender.OnSendInputValue -= AttacherChangeAttached;
        }
    }

    public override bool GetBool()
    {
        return IsUp;
    }

    private void AttacherChangeAttached(float value)
    {
        IsUp = value > 0.5f;
    }
}
