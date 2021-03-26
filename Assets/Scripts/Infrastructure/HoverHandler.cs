using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CustomCursorType
{
    DEFAULT,
    DRAGABLE,
    DRAGGING,
}

public class HoverHandler : Singleton<HoverHandler>
{
    CustomCursorType customCursor;
    IHoverable currentHover;
    [SerializeField] [Expandable] Effect onHoverEnter, onHoverExit;
    [SerializeField] Texture2D defaultCursor, dragableCursor, dragggingCursor;

    private void OnEnable()
    {
        Game.DragHandler.OnStartDrag += OnStartDrag;
        Game.DragHandler.OnEndDrag += OnEndDrag;
    }

    private void OnDisable()
    {
        Game.DragHandler.OnStartDrag -= OnStartDrag;
        Game.DragHandler.OnEndDrag -= OnEndDrag;
    }

    public void UpdateHover(RaycastHit hit, IDragable dragable)
    {
        //Hover
        IHoverable newDragHover = null;
        if (hit.collider != null)
            newDragHover = hit.collider.GetComponent<IHoverable>();

        if (currentHover != newDragHover)
        {
            if (currentHover != null)
            {
                Game.EffectHandler.Play(onHoverExit, currentHover.GetGameObject());
                currentHover.EndHover();
            }

            if (newDragHover != null)
            {
                Game.EffectHandler.Play(onHoverEnter, newDragHover.GetGameObject());
                SetCursorType(CustomCursorType.DRAGABLE);
                newDragHover.StartHover();
            } else
            {
                SetCursorType(CustomCursorType.DEFAULT);
            }

            currentHover = newDragHover;
        }
    }
    public void ForceEndHover()
    {
        if (currentHover != null)
        {
            Game.EffectHandler.Play(onHoverExit, currentHover.GetGameObject());
            currentHover.EndHover();
            currentHover = null;
        }
    }

    internal void OnStartDrag(IDragable dragable, RaycastHit hit)
    {
        SetCursorType(CustomCursorType.DRAGGING);
    }

    internal void OnEndDrag()
    {
        SetCursorType(CustomCursorType.DEFAULT);
        currentHover = null;
    }

    private void SetCursorType(CustomCursorType customCursorType)
    {
        if (customCursor != customCursorType)
        {
            Cursor.SetCursor(GetCursorTextureByType(customCursorType), Vector2.zero, CursorMode.Auto);
            customCursor = customCursorType;
        }
    }

    private Texture2D GetCursorTextureByType(CustomCursorType customCursorType)
    {
        switch (customCursorType)
        {
            case CustomCursorType.DRAGABLE:
                return dragableCursor;
                break;

            case CustomCursorType.DRAGGING:
                return dragggingCursor;
                break;
        }

        return defaultCursor; 
    }

    private void OnDrawGizmos()
    {
        if (currentHover != null)
        {
            DebugDraw.Circle(currentHover.GetGameObject().transform.position, Color.white, 0.5f);
        }
    }
}
