using Cinemachine;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ZoomIn))]
public class ZoomInChangeCamera : ConditionedObject
{
    ZoomIn zoomIn;
    [SerializeField] CinemachineVirtualCamera cameraOne, cameraTwo;

    protected override void OnEnable()
    {
        zoomIn = GetComponent<ZoomIn>();
        base.OnEnable();
    }

    public override bool Try()
    {
        if (base.Try())
        {
            cameraTwo.Priority = 100;
            cameraOne.Priority = 10;
            zoomIn.OverrideVirtualCamera(cameraTwo);
            return true;
        }

        return false;
    }
}
