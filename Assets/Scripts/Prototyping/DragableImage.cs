using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class DragableImage : MovingObject, IDragable
{
    [OnValueChanged("PreviewUpperPosition")]
    [SerializeField] Vector3 localPositionUpper;

    [Dropdown("VectorValues")]
    public Vector3 forward;
    private Vector3[] VectorValues = new Vector3[] { Vector3.right, Vector3.left, Vector3.forward, Vector3.back, Vector3.up, Vector3.down };

    [TextArea] [SerializeField] string tooltipText;

    [Foldout("Effects")] [Expandable] [SerializeField] Effect startUseEffect, endUseEffect;

    Vector3 localPositionOnStartDrag;
    Vector3 clickPositonOnStartDrag;

    float distanceBetweenUpperAndLower;
    private bool pulledOut;

    public bool IsNull => this == null;

    public event Action OnStartHoverEvent;
    public event Action OnEndHoverEvent;

#if UNITY_EDITOR
    private void PreviewUpperPosition()
    {
        objectToMove.localPosition = localPositionUpper;
    }
    protected override void Reset ()
    {

    }

    [Button]
    private void SetCurrentChildPositionAsUpper ()
    {
        localPositionUpper = objectToMove.localPosition;
    }

    [Button]
    private void SetCurrentChildPositionAsLower()
    {
        localPositionTrue = objectToMove.localPosition;
    }
#endif
    private void Start()
    {
        objectToMove.localPosition = localPositionTrue;
        distanceBetweenUpperAndLower = Vector3.Distance(localPositionUpper, localPositionTrue);
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
        return forward;
    }

    public void StartDrag()
    {
        localPositionOnStartDrag = objectToMove.localPosition;
        clickPositonOnStartDrag = Vector3.zero;
        Game.EffectHandler.Play(startUseEffect,gameObject);
    }


    public void UpdateDragPositionAndRotation(Vector3 point, Vector3 vector3, bool useCustomPivot, Quaternion rotation)
    {
        if (clickPositonOnStartDrag == Vector3.zero)
            clickPositonOnStartDrag = point;

        float dragDistance = (clickPositonOnStartDrag - point).FilterByAxis((localPositionUpper - localPositionTrue).normalized);
        Vector3 lerp = (localPositionTrue - localPositionUpper) * (dragDistance / distanceBetweenUpperAndLower);
        objectToMove.localPosition = (localPositionOnStartDrag + (lerp)).Clamp(localPositionTrue, localPositionUpper);
        pulledOut = true;
    }

    protected override void UpdateChange(float progress)
    {
        if (pulledOut)
        {
            objectToMove.localPosition = Vector3.MoveTowards(objectToMove.localPosition, Vector3.Lerp(localPositionFalse, localPositionTrue, progress), UnityEngine.Time.deltaTime);
            if (progress < 0.05f)
                pulledOut = false;
        }
        else
        {
            base.UpdateChange(progress);
        }
}

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + localPositionUpper, transform.position + localPositionUpper + forward);
        Gizmos.DrawLine(transform.position + localPositionTrue, transform.position + localPositionTrue + forward);
        Gizmos.DrawLine(transform.position + localPositionUpper + forward * 0.33f, transform.position + localPositionTrue + forward * 0.33f);
    }

    public string GetTooltipText()
    {
        return tooltipText;
    }
}
