using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public interface IHoverable
{
    bool IsNull { get; }
    void StartHover();
    void EndHover();
    event Action OnStartHoverEvent;
    event Action OnEndHoverEvent;
    GameObject gameObject { get; }
}

public interface IDragable : IHoverable
{
    void StartDrag();
    void EndDrag(Vector3 position);
    float GetEndDragYOffset();
    bool IsDragable();
    bool ShouldLockOnDrag();
    Vector3 GetRaycastPlaneLockDirection();
    void UpdateDragPosition(Vector3 point, Vector3 vector3, bool useCustomPivot);
}

public interface IAttachable : IDragable
{
    void Attach(IAttacher toAttachTo);
    string GetAttachment();
    IAttacher GetCurrentAttached();
    InventoryObjectData GetInventoryObjectData();
}

public class SimpleDragable : MonoBehaviour, IDragable
{
    protected Rigidbody myRigidbody;
    [SerializeField] protected Transform customDragPivot;
    [SerializeField] private float YOffsetOnDrop;

    [Foldout("Effects")] [Expandable] [SerializeField] protected Effect startDragEffect, endDragEffect;

    protected bool isBeeingDragged = false;

    public bool IsNull => this == null;

    public event Action OnStartHoverEvent;
    public event Action OnEndHoverEvent;

    protected virtual void OnEnable()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            myRigidbody = rb;
    }

    protected virtual void SetPhysicsActive(bool active)
    {
        if (myRigidbody == null)
            return;

        if (active)
            myRigidbody.constraints = RigidbodyConstraints.FreezeRotationY;
        else
            myRigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    protected virtual void SetMouseRaycastable(bool raycastable)
    {
        gameObject.layer = raycastable ? 0 : Physics.IgnoreRaycastLayer;
    }

    public virtual bool IsDragable()
    {
        return !isBeeingDragged;
    }
    public virtual void StartDrag()
    {
        isBeeingDragged = true;
        SetPhysicsActive(false);
        SetMouseRaycastable(false);
        Game.EffectHandler.Play(startDragEffect, gameObject);
    }
    public virtual void EndDrag(Vector3 position)
    {
        isBeeingDragged = false;
        transform.position = position;
        SetPhysicsActive(true);
        SetMouseRaycastable(true);
        Game.EffectHandler.Play(endDragEffect, gameObject);
    }
    public void UpdateDragPosition(Vector3 hitpoint, Vector3 position, bool useCustomPivot)
    {
        transform.position = position - ((!useCustomPivot || customDragPivot == null) ? Vector3.zero : (customDragPivot.position - transform.position));
    }

    public float GetEndDragYOffset()
    {
        return YOffsetOnDrop;
    }

    public void StartHover()
    {
        OnStartHoverEvent?.Invoke();
    }

    public void EndHover()
    {
        OnEndHoverEvent?.Invoke();
    }

    public bool ShouldLockOnDrag()
    {
        return false;
    }

    public Vector3 GetRaycastPlaneLockDirection()
    {
        return Vector3.zero;
    }
}
