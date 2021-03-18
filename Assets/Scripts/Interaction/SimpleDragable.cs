using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHoverable
{
    void StartHover();
    void EndHover();
    event Action OnStartHoverEvent;
    event Action OnEndHoverEvent;
    GameObject GetGameObject();

}

public interface IDragable
{
    void StartDrag();
    void EndDrag(Vector3 position);
    float GetEndDragYOffset();
    bool IsDragable();
    void UpdateDragPosition(Vector3 point, Vector3 vector3);
}

public interface IAttachable : IDragable
{
    void Attach(IAttacher toAttachTo);
    string GetAttachment();
    IAttacher GetCurrentAttached();
}

public class SimpleDragable : MonoBehaviour, IDragable, IHoverable
{
    [SerializeField] protected Rigidbody rigidbody;
    [SerializeField] private float YOffsetOnDrop;
    [SerializeField] protected AudioClip startDragClip, endDragClip;


    protected bool isBeeingDragged = false;

    public event Action OnStartHoverEvent;
    public event Action OnEndHoverEvent;

    protected virtual void SetPhysicsActive(bool active)
    {
        if (active)
            rigidbody.constraints = RigidbodyConstraints.FreezeRotationY;
        else
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
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
        Game.SoundPlayer.Play(startDragClip, gameObject);
    }
    public virtual void EndDrag(Vector3 position)
    {
        isBeeingDragged = false;
        transform.position = position;
        SetPhysicsActive(true);
        SetMouseRaycastable(true);
        Game.SoundPlayer.Play(endDragClip, gameObject);
    }
    public void UpdateDragPosition(Vector3 hitpoint, Vector3 position)
    {
        transform.position = position;
    }

    public float GetEndDragYOffset()
    {
        return YOffsetOnDrop;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void StartHover()
    {
        OnStartHoverEvent?.Invoke();
    }

    public void EndHover()
    {
        OnEndHoverEvent?.Invoke();
    }
}
