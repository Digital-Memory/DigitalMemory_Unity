using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionListenerAttacher : ConditionListenerBehaviour
{
    Attacher attacher;
    public bool IsAttached;

    void OnEnable()
    {
        attacher = GetComponent<Attacher>();
        if (attacher != null)
        {
            attacher.OnChangeAttached += AttacherChangeAttached;
        }
    }

    void OnDisable()
    {
        if (attacher != null)
        {
            attacher.OnChangeAttached -= AttacherChangeAttached;
        }
    }

    public override bool GetBool()
    {
        return IsAttached;
    }

    private void AttacherChangeAttached(bool isAttached, string attachment)
    {
        IsAttached = isAttached;
    }
}
