using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Crank : SimpleAttachable
{
    [Expandable] [SerializeField] Effect crankBlockedEffect;

    [ShowNonSerializedField] FloatSender floatSender;
    [ShowNonSerializedField] float angleBefore;

    public override void Attach(IAttacher toAttachTo)
    {
        base.Attach(toAttachTo);
        floatSender = GetComponentInParent<FloatSender>();
        if (floatSender != null)
            floatSender.OnSendCallbackWithFactor += SetRotationFromAngle;
        Debug.LogWarning("attached to " + toAttachTo.GetTransform().gameObject.name);
        ResetAngleBefore();
    }

    public override void StartDrag()
    {
        base.StartDrag();
        if (floatSender != null)
            floatSender.OnSendCallbackWithFactor -= SetRotationFromAngle;
        floatSender = null;
    }

    internal void TryTurnTo(float angle)
    {
        if (floatSender == null)
        {
            Debug.LogError("No parent attacher found...");
            return;
        }

        if (angleBefore == float.MinValue)
            angleBefore = angle;

        float deltaAngle = Mathf.DeltaAngle(angleBefore, angle);
        Debug.Log("deltaAngle between: " + deltaAngle + " (" + angleBefore + " / " + angle + ")");

        if (Mathf.Abs(deltaAngle) > 45f)
        {
            Game.EffectHandler.Play(crankBlockedEffect, gameObject);
            Game.DragHandler.ForceEndDrag();
        }
        else
        {
            if (floatSender.TryGiveInput(deltaAngle))
            {
                //Rotate(angle - angleBefore);
                //angleBefore = angle;
                //SetRotationFromAngle(angle);
            }
            else
            {
                Game.EffectHandler.Play(crankBlockedEffect, gameObject);
            }
        }
    }

    private void SetRotationFromAngle(float angle)
    {
        angleBefore = angle;
        float angleCorrected = angle;
        Debug.Log("Set Rotatation: " + angleCorrected);
        transform.rotation = Quaternion.Euler(0, angleCorrected, 0);
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
