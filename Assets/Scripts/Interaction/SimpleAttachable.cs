using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICloseupable
{
    Vector3 GetPosition();
    Quaternion GetRotation();
    void UpdatePositionAndRotation(Vector3 pos, Quaternion rot);
    void OnStartCloseup();
    void OnEndCloseup();

    event Action OnStartCloseupEvent;
    event Action OnEndCloseupEvent;

    bool ShouldOffset();
}


[SelectionBase]
public class SimpleAttachable : SimpleDragable, IAttachable, ICloseupable
{
    public string attachment;
    private bool isInCloseup;
    [SerializeField] private bool isAttached;
    [SerializeField] private Transform defaultParent;

    [Expandable]
    [SerializeField] protected Effect attachEffect, detachEffect, potentialSlotEffect;

    public event Action OnStartCloseupEvent;
    public event Action OnEndCloseupEvent;

    void Start()
    {
        if (isAttached)
        {
            IAttacher parent = GetComponentInParent<IAttacher>();

            if (parent != null)
            {
                Attach(parent);
                parent.OnAttach(this);
            }
            else
            {
                Debug.LogError("No parent attacher found, uncheck the bool or find a proper parent");
            }
        }
    }

    public virtual void Attach(IAttacher toAttachTo)
    {
        isBeeingDragged = false;
        isAttached = true;

        defaultParent = transform.parent;
        toAttachTo.HandleTransformOnAttach(transform);

        SetMouseRaycastable(true);
        SetPhysicsActive(false);

        Game.EffectHandler.Play(attachEffect, gameObject);
        Game.EffectHandler.StopOnAllPotentialAttachables(attachment);
    }

    public override void StartDrag()
    {
        Game.EffectHandler.PlayOnAllPotentialAttachables(potentialSlotEffect, attachment);
        base.StartDrag();

        if (isAttached)
        {
            Game.EffectHandler.Play(detachEffect, gameObject);
            transform.localScale = Vector3.one;

            isAttached = false;
            if (defaultParent != null)
                transform.parent = defaultParent;
            else
            {
                transform.parent = null;
                Debug.LogWarning("No default parent defined unparenting impossible, please define one or ignore this warning.");
            }
        }
    }

    public override void EndDrag(Vector3 position)
    {
        base.EndDrag(position);
        Game.EffectHandler.StopOnAllPotentialAttachables(attachment);
    }
    public string GetAttachment()
    {
        return attachment;
    }

    public IAttacher GetCurrentAttached()
    {
        if (isAttached)
            return GetComponentInParent<IAttacher>();

        return null;
    }
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Quaternion GetRotation()
    {
        return transform.rotation;
    }

    public void UpdatePositionAndRotation(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
    }

    public virtual void OnStartCloseup()
    {
        isInCloseup = true;
        SetPhysicsActive(false);
        SetMouseRaycastable(false);
        OnStartCloseupEvent?.Invoke();
    }

    public virtual void OnEndCloseup()
    {
        isInCloseup = false;
        SetMouseRaycastable(true);
        if (!isAttached)
            SetPhysicsActive(true);
        OnEndCloseupEvent?.Invoke();
    }

    public override bool IsDragable()
    {
        return base.IsDragable() && !isInCloseup;
    }

    public bool ShouldOffset()
    {
        TextDisplayer textDisplayer = GetComponent<TextDisplayer>();
        return (textDisplayer != null && textDisplayer.HasCloseupText);
    }
}
