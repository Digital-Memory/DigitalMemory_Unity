using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Lever : SimpleAttachable
{
    [ShowNonSerializedField] AttacherLever currentAttacher;
    [SerializeField] float snapDistance;

    public override void Attach(IAttacher toAttachTo)
    {
        base.Attach(toAttachTo);
        currentAttacher = GetComponentInParent<AttacherLever>();
        transform.localRotation = currentAttacher.GetLeverRotationForCurrentPosition();

        Debug.LogWarning("attached to " + currentAttacher.name);
    }

    public override void StartDrag()
    {
        base.StartDrag();
        currentAttacher = null;
    }

    public override void EndDrag(Vector3 position)
    {
        base.EndDrag(position);
    }

    internal void Turn(float angle)
    {
        if (currentAttacher != null)
        {
            float targetAngle = currentAttacher.isTop ? currentAttacher.topRotation : currentAttacher.bottomRotation;
            transform.localRotation = Quaternion.Euler(0, 0, targetAngle > angle ? Mathf.Min(targetAngle, angle) : Mathf.Max(targetAngle, angle));

            if (Mathf.Abs(Mathf.DeltaAngle(angle, targetAngle)) < snapDistance)
            {
                currentAttacher.Switch(angle);
            }
        }
    }
}
