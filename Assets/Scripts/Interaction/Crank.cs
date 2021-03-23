using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Crank : SimpleAttachable
{
    [Expandable] [SerializeField] Effect crankBlockedEffect;

    [ShowNonSerializedField] AttacherCrank currentAttacher;
    float angleBefore;

    public override void Attach(IAttacher toAttachTo)
    {
        base.Attach(toAttachTo);
        currentAttacher = GetComponentInParent<AttacherCrank>();
        Debug.LogWarning("attached to " + currentAttacher.name);
        ResetAngleBefore();
    }

    public override void StartDrag()
    {
        base.StartDrag();
        currentAttacher = null;
    }

    internal void TryTurnTo(float angle)
    {
        if (currentAttacher == null)
        {
            Debug.LogError("No parent attacher found...");
            return;
        }

        if (angleBefore == float.MinValue)
            angleBefore = angle;

        float deltaAngle = Mathf.DeltaAngle(angleBefore, angle);
        //Debug.Log("deltaAngle between: "+ deltaAngle + " (" + angleBefore + " / " + angle + ")");

        if (Mathf.Abs(deltaAngle) > 45f)
        {
            Game.EffectHandler.Play(crankBlockedEffect, gameObject);
            Game.MouseInteractor.ForceEndDrag();
        }
        else
        {
            if (currentAttacher.TryRotate(deltaAngle))
            {
                transform.Rotate(Vector3.up, angle - angleBefore);
                angleBefore = angle;
            }
            else
            {
                Game.EffectHandler.Play(crankBlockedEffect, gameObject);
            }
        }
    }

    public void ResetAngleBefore()
    {
        angleBefore = float.MinValue;
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
