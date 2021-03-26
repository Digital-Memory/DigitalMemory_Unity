using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionListenerCrank : ConditionListenerBehaviour
{
    AttacherCrank attacher;
    float value;

    void OnEnable()
    {
        attacher = GetComponent<AttacherCrank>();
        if (attacher != null)
        {
            attacher.OnChangeValue += AttacherChangeAttached;
        }
    }

    void OnDisable()
    {
        if (attacher != null)
        {
            attacher.OnChangeValue -= AttacherChangeAttached;
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
