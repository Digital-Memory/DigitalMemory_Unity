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
            angle = angle % 360;
            if (angle > 180)
                angle = angle - 360;

            float targetAngle = Mathf.Sign(angle) == -1f ? currentAttacher.topRotation : currentAttacher.bottomRotation;
            float targetAngleClamped = Mathf.Sign(angle) == -1f ? Mathf.Max(targetAngle, angle) : Mathf.Min(targetAngle, angle);
            //Debug.Log(angle + "" + targetAngle + " = " + targetAngleClamped);
            transform.localRotation = Quaternion.Euler(0, 0, targetAngleClamped);

            if (Mathf.Abs(Mathf.DeltaAngle(angle, targetAngleClamped)) < snapDistance)
            {
                if (currentAttacher.isTop == (Mathf.Sign(angle) == -1f))
                currentAttacher.Switch(angle);
            }
        }
    }

    public Vector2 GetMinMaxRotations ()
    {
        if (currentAttacher != null)
            return new Vector2(currentAttacher.bottomRotation, currentAttacher.topRotation);
        else
            return Vector2.zero;
    }
}
