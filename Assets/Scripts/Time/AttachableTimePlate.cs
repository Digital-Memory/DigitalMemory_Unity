using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttachableTimePlate : SimpleAttachable
{
    [SerializeField] public TimePoint TimePoint;
    [SerializeField] TMP_Text tMP_Text;

    protected override void OnEnable()
    {
        base.OnEnable();
        tMP_Text.text = TimePoint.ToString();
    }

    protected override void SetMouseRaycastable(bool raycastable)
    {
        //Need to rework this at some point to be fixed for alle objects
    }
}
