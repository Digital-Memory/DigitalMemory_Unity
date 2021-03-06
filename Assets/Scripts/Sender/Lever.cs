using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class Lever : SimpleAttachable
{
    FloatSender floatSender;
    FloatSnapper floatSnapper;

    [Foldout("Configuration")] [SerializeField] Vector3 rotationAxis = Vector3.forward;
    [Foldout("Configuration")] [SerializeField] Vector3 rotationOffsett = Vector3.zero;

    public override void Attach(IAttacher toAttachTo)
    {
        base.Attach(toAttachTo);
        floatSender = GetComponentInParent<FloatSender>();
        if (floatSender != null)
        {
            SetRotationFromAngle(floatSender.Factorize(floatSender.CurrentValue));
            floatSender.OnSendCallbackWithFactor += SetRotationFromAngle;
        }

        floatSnapper = GetComponentInParent<FloatSnapper>();
    }

    private void SetRotationFromAngle(float angle)
    {
        transform.localRotation = Quaternion.Euler(angle * rotationAxis + rotationOffsett);
    }

    public override void StartDrag()
    {
        base.StartDrag();

        if (floatSender != null)
            floatSender.OnSendCallbackWithFactor -= SetRotationFromAngle;

        floatSender = null;
        floatSnapper = null;
    }

    public void StartPlayerInput()
    {
        floatSender.StartPlayerInput();
    }

    public void EndPlayerInput()
    {
        floatSender.EndPlayerInput();
    }

    public override void EndDrag(Vector3 position)
    {
        base.EndDrag(position);
    }

    public virtual void Turn(float angle)
    {
        float a = angle;

        if (floatSender != null)
        {
            angle = angle % 360;
            if (angle > 180)
                angle = angle - 360;

            floatSender.TryGiveInput(angle, isAbsolute: true);
        }
    }
}
