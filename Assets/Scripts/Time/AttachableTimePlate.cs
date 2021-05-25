using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachableTimePlate : SimpleAttachable
{
    [SerializeField] public TimePoint TimePoint;

    protected override void SetMouseRaycastable(bool raycastable)
    {
        //Need to rework this at some point to be fixed for alle objects
    }
}
