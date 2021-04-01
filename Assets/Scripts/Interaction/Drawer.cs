using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour, IDragable, IAttacher
{
    bool isDragging = false;
    Vector3 startDragPosition;
    [SerializeField] float drawerSpeedMultiplier;
    [SerializeField] Vector3 dragAxis, position, current, target, minDrag, maxDrag;
    [SerializeField] Vector3 attachOffset;

    public event Action OnStartHoverEvent;
    public event Action OnEndHoverEvent;

    public void EndDrag(Vector3 position)
    {
        SetPulledOut(startDragPosition, position);
        target = Vector3.zero;
        isDragging = false;
    }

    private void SetPulledOut(Vector3 startDragPosition, Vector3 currentDragPosition)
    {
        Vector3 currentDrag = (currentDragPosition.FilterByAxisVector(dragAxis) - startDragPosition.FilterByAxisVector(dragAxis));
        current = (position + currentDrag).FilterByAxisVector(dragAxis);
        if (current.FilterByAxis(dragAxis) <= minDrag.FilterByAxis(dragAxis) && current.FilterByAxis(dragAxis) >= maxDrag.FilterByAxis(dragAxis))
            target = position + currentDrag;
    }

    public float GetEndDragYOffset()
    {
        return 0;
    }

    public bool IsDragable()
    {
        return !isDragging;
    }

    public void StartDrag()
    {
        isDragging = true;
        position = transform.localPosition;
        startDragPosition = Vector3.zero;
    }

    public void UpdateDragPosition(Vector3 hitpoint, Vector3 position)
    {
        if (startDragPosition == Vector3.zero)
            startDragPosition = hitpoint;
        else
        {
            SetPulledOut(startDragPosition, hitpoint);
        }
    }

    void FixedUpdate()
    {
        if (target != Vector3.zero)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, drawerSpeedMultiplier);
        }
    }

    public bool CanAttach(string attachBehaviour)
    {
        return true;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void OnAttach(IAttachable attachable)
    {
        //
    }

    public void OnDetach()
    {
        //
    }

    public Vector3 GetPreviewPosition(Vector3 point)
    {
        return point;
    }

    public bool ResetPositionOnAttach()
    {
        return false;
    }

    public bool ResetOrientationOnAttach()
    {
        return false;
    }

    public Vector3 GetAttachOffset()
    {
        return attachOffset;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + attachOffset, new Vector3(10,1,10));
    }

    public bool ShouldLockOnDrag()
    {
        return true;
    }

    public void StartHover()
    {
        OnStartHoverEvent?.Invoke();
    }

    public void EndHover()
    {
        OnEndHoverEvent?.Invoke();
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void HandleTransformOnAttach(Transform transformAttached)
    {
        transformAttached.parent = transform;
        transformAttached.localPosition += attachOffset;
    }
}
