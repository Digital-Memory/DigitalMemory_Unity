using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PullableImageFrame : MonoBehaviour, IDragable
{
    Vector3 localPositionOnStartDrag;
    Vector3 clickPositonOnStartDrag;

    Vector3 localPositionDefault = Vector3.zero;
    [SerializeField] Vector3 localPositionPulledOut;

    float distanceBetweenUpperAndLower;
    private bool pulledOut;

    public bool IsNull => this == null;

    public event Action OnStartHoverEvent;
    public event Action OnEndHoverEvent;

    [Foldout("Effects")] [Expandable] [SerializeField] Effect startUseEffect, endUseEffect;
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
    public Vector3 GetRaycastPlaneLockDirection()
    {
        return transform.forward;
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

        float dragDistance = (clickPositonOnStartDrag - point).FilterByAxis((localPositionPulledOut - localPositionDefault).normalized);
        Vector3 lerp = (localPositionDefault - localPositionPulledOut) * (dragDistance / distanceBetweenUpperAndLower);
        transform.localPosition = (localPositionOnStartDrag + (lerp)).Clamp(localPositionDefault, localPositionPulledOut);
    }
}
