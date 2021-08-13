using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public enum DrawerElementLockDirection
{
    Forward,
    Up,
}

public class DrawerElement: MonoBehaviour, IDragable
{
    Vector3 localPositionOnStartDrag;
    Vector3 clickPositonOnStartDrag;

    Vector3 localPositionDefault = Vector3.zero;
    [InfoBox("0 0 0 is takes as default local postion, so make sure it is at 0 0 0.")]
    [SerializeField] Vector3 localPositionPulledOut;

    [TextArea] [SerializeField] string tooltipText;

    float distanceBetweenUpperAndLower;
    private bool pulledOut;

    public bool IsNull => this == null;

    public event Action OnStartHoverEvent;
    public event Action OnEndHoverEvent;

    [Foldout("Effects")] [Expandable] [SerializeField] Effect startUseEffect, endUseEffect;
    [Foldout("Configuration")] [SerializeField] DrawerElementLockDirection lockDirection = DrawerElementLockDirection.Forward;
    [Foldout("Configuration")] [SerializeField] bool invert = false;
    private void OnEnable()
    {
        distanceBetweenUpperAndLower = Vector3.Distance(localPositionPulledOut, localPositionDefault);
    }
    public void EndDrag(Vector3 position)
    {
        Game.EffectHandler.Play(endUseEffect, gameObject);
    }
    public void StartHover()
    {
        OnStartHoverEvent?.Invoke();
    }
    public void EndHover()
    {
        OnEndHoverEvent?.Invoke();
    }

    public float GetEndDragYOffset()
    {
        return 0f;
    }

    public bool IsDragable()
    {
        return true;
    }

    public bool ShouldLockOnDrag()
    {
        return true;
    }
    public Vector3 GetRaycastPlaneLockDirection(Vector3 point)
    {
        if (lockDirection == DrawerElementLockDirection.Forward)
            return transform.forward;
        else
            return transform.up;
    }

    public void StartDrag()
    {
        localPositionOnStartDrag = transform.localPosition;
        clickPositonOnStartDrag = Vector3.zero;
        Game.EffectHandler.Play(startUseEffect, gameObject);
    }


    public void UpdateDragPositionAndRotation(Vector3 point, Vector3 vector3, bool useCustomPivot, Quaternion rotation)
    {
        if (clickPositonOnStartDrag == Vector3.zero)
            clickPositonOnStartDrag = point;

        float dragDistance = ((invert ? -1 : 1 ) * (clickPositonOnStartDrag - point)).FilterByAxis((localPositionPulledOut - localPositionDefault).normalized);
        Vector3 lerp = (localPositionDefault - localPositionPulledOut) * (dragDistance / distanceBetweenUpperAndLower);
        transform.localPosition = (localPositionOnStartDrag + (lerp)).Clamp(localPositionDefault, localPositionPulledOut);
    }

    public string GetTooltipText()
    {
        return tooltipText;
    }
}
