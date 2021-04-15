using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverHandler : Singleton<HoverHandler>
{
    IHoverable currentHover;
    [SerializeField] [Expandable] Effect onHoverEnter, onHoverExit;

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
            if (currentHover != null && !currentHover.IsNull)
            {
                Game.EffectHandler.Play(onHoverExit, currentHover.GetGameObject());
                currentHover.EndHover();
            }

            if (newDragHover != null)
            {
                Game.EffectHandler.Play(onHoverEnter, newDragHover.GetGameObject());
                Game.UIHandler.CustomCursor.SetCursorType(CustomCursorType.DRAGABLE);
                newDragHover.StartHover();
            }
            else
            {
                Game.UIHandler.CustomCursor.SetCursorType(CustomCursorType.DEFAULT);
            }

            currentHover = newDragHover;
        }
    }

    internal void EnterUI()
    {
        if (Game.DragHandler.IsDragging)
        {
            IInventoryObject inventoryObject = Game.DragHandler.GetDragging().GetComponent<IInventoryObject>();

            if (inventoryObject != null)
            {
                Game.UIHandler.CustomCursor.SetCursorType(CustomCursorType.MANUAL ,inventoryObject.GetData().icon, 4f);
                Game.DragHandler.HideDragging();
            }
        }
    }
    internal void ExitUI()
    {
        Game.UIHandler.CustomCursor.ResetCursor(CustomCursorType.DEFAULT, CustomCursorType.DRAGABLE, CustomCursorType.DRAGGING);
        Game.DragHandler.ShowDragging();
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
        Game.UIHandler.CustomCursor.SetCursorType(CustomCursorType.DRAGGING);
    }

    internal void OnEndDrag(IDragable dragable)
    {
        Game.UIHandler.CustomCursor.SetCursorType(CustomCursorType.DEFAULT);
        currentHover = null;
    }

    private void OnDrawGizmos()
    {
        if (currentHover != null && currentHover.GetGameObject() != null)
        {
            DebugDraw.Circle(currentHover.GetGameObject().transform.position, Color.white, 0.5f);
        }
    }

    private void OnGUI()
    {
        string name = currentHover != null ? currentHover.GetGameObject().name : "";
        GUI.Box(new Rect(10,10,100,50), name);
    }
}
