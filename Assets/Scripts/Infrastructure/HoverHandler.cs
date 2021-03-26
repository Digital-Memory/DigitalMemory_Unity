using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverHandler : Singleton<HoverHandler>
{
    IHoverable currentHover;
    [SerializeField] [Expandable] Effect onHoverEnter, onHoverExit;
    [SerializeField] Texture2D defaultCursor, dragableCursor, dragggingCursor;

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
                Cursor.SetCursor(dragableCursor, Vector2.zero, CursorMode.Auto);
                newDragHover.StartHover();
            } else
            {
                Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
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

    internal void NotifyStartDrag()
    {
        Cursor.SetCursor(dragggingCursor, Vector2.zero, CursorMode.Auto);
    }

    internal void NotifyEndDrag()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        currentHover = null;
    }

    private void OnDrawGizmos()
    {
        if (currentHover != null)
        {
            DebugDraw.Circle(currentHover.GetGameObject().transform.position, Color.yellow, 0.5f);
        }
    }

    private void OnGUI()
    {
        if (currentHover != null)
        {
            GUILayout.Box(currentHover.GetGameObject().name);
        }
    }
}
