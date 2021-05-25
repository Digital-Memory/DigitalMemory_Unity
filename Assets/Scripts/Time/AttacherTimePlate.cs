using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttacherTimePlate : Attacher
{
    [Range(0,1)]
    [SerializeField] float leverPosition;

    public System.Action<float, TimePoint> OnAttachPlateOnPosition;
    public System.Action<float> OnDetachPlateOnPosition;

    public override void OnAttach(IAttachable attachable)
    {
        base.OnAttach(attachable);
        AttachableTimePlate attached = attachable.gameObject.GetComponent<AttachableTimePlate>();
        if (attached != null)
            OnAttachPlateOnPosition?.Invoke(leverPosition, attached.TimePoint);
    }

    public override void OnDetach()
    {
        base.OnDetach();
        OnDetachPlateOnPosition?.Invoke(leverPosition);
    }

    public TimePoint GetAttachedTimePoint()
    {
        return GetComponentInChildren<AttachableTimePlate>().TimePoint;
    }

    public float GetAssociatedPosition()
    {
        return leverPosition;
    }
}
