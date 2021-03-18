using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Crank : SimpleAttachable
{
    [ShowNonSerializedField] AttacherCrank currentAttacher;
    float angleBefore;

    public override void Attach(IAttacher toAttachTo)
    {
        base.Attach(toAttachTo);
        currentAttacher = GetComponentInParent<AttacherCrank>();
        Debug.LogWarning("attached to " + currentAttacher.name);
        angleBefore = float.MinValue;
    }

    public override void StartDrag()
    {
        base.StartDrag();
        currentAttacher = null;
    }

    internal void Turn(float angle)
    {
        if (currentAttacher == null)
        {
            Debug.LogError("No parent attacher found...");
            return;
        }

        //Debug.Log("angle: " + angle + " (relative: " + (angle - angleBefore) + ")");

        if (angleBefore == float.MinValue)
            angleBefore = angle;

        if (currentAttacher.TryTurn(Mathf.DeltaAngle(angleBefore, angle)))
        {
            transform.Rotate(Vector3.up, angle - angleBefore);
            angleBefore = angle;
        }
    }

#if UNITY_EDITOR

    [Button]
    private void AttachToParent()
    {
        IAttacher parent = GetComponentInParent<IAttacher>();

        if (parent != null)
        {
            Attach(parent);
            parent.OnAttach(this);
        }
        else
        {
            Debug.LogError("No parent attacher found");
        }
    }

#endif
}
