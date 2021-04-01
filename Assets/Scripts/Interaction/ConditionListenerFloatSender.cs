using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionListenerFloatSender : ConditionListenerBehaviour
{
    FloatSender floatSender;
    float value;

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

    public override float GetFloat()
    {
        return value;
    }

    private void AttacherChangeAttached(float value)
    {
        this.value = value;
    }
}
