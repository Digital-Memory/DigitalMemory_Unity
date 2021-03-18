using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionLever : ConditionBehaviour
{
    AttacherLever attacher;
    public bool IsUp;

    void OnEnable()
    {
        attacher = GetComponent<AttacherLever>();
        if (attacher != null)
        {
            attacher.OnLeverTurn += AttacherChangeAttached;
        }
    }

    void OnDisable()
    {
        if (attacher != null)
        {
            attacher.OnLeverTurn -= AttacherChangeAttached;
        }
    }

    public override bool GetBool()
    {
        return IsUp;
    }

    private void AttacherChangeAttached(bool isUp)
    {
        IsUp = isUp;
    }
}
