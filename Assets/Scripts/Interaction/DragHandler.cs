using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragHandler : Singleton<DragHandler>
{
    IDragable currentDrag;
    IAttachable currentAttachable;

    public bool IsDragging { get => (currentDrag != null); }
    public bool IsDraggingAttachable { get => (currentAttachable != null); }

    public event System.Action<IDragable, RaycastHit> OnStartDrag;
    public event System.Action<IDragable> OnEndDrag;

    internal void ShowDragging()
    {
        if (IsDragging)
            currentDrag.gameObject.layer = LayerMask.NameToLayer("Water");
    }
    internal void HideDragging()
    {
        if (IsDragging)
            currentDrag.gameObject.layer = LayerMask.NameToLayer("Hidden");
    }

    internal GameObject GetDragging()
    {
        return currentDrag.gameObject;
    }


    public void UpdateDrag(RaycastHit hit, Ray ray)
    {
        IAttacher attacher = null;

        if (hit.collider != null)
            attacher = hit.collider.GetComponent<IAttacher>();

        bool IsAboveAttacher = attacher != null;
        bool CanAttach = IsAboveAttacher ? attacher.CanAttach(currentAttachable.GetAttachment()) : false;

        if (IsDraggingAttachable && IsAboveAttacher)
        {
            if (CanAttach)
            {
                //attachment preview
                currentDrag.UpdateDragPositionAndRotation(hit.point, CalculateAnimatedPreviewPosition(hit.point, attacher), useCustomPivot: false, attacher.GetTransform().rotation);
                Game.UIHandler.CustomCursor.SetCursorType(CustomCursorType.DRAGGING);
            }
            else
            {
                UpdateDragPreviewRegular(hit, ray);
                Game.UIHandler.CustomCursor.SetCursorType(CustomCursorType.X);
            }
        }
        else
        {
            UpdateDragPreviewRegular(hit, ray);
            Game.UIHandler.CustomCursor.SetCursorType(CustomCursorType.DRAGGING);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (IsDraggingAttachable && CanAttach)
            {
                //attach
                Attach(currentAttachable, attacher);
            }
            else
            {
                //drop
                EndDrag(currentDrag, hit.point);
            }
        }
    }


    private void UpdateDragPreviewRegular(RaycastHit hit, Ray ray)
    {
        //regular drag
        float dragDistance = Vector3.Distance(ray.origin, hit.point) - Game.Settings.DragDistanceToFloor * Game.Settings.CurrentZoomLevel;
        currentDrag.UpdateDragPositionAndRotation(hit.point, ray.GetPoint(dragDistance), useCustomPivot: false, Quaternion.identity);
    }

    public void StartDrag(RaycastHit hit, IDragable dragable, IAttachable attachable)
    {
        Debug.Log($"start dragging: {dragable.gameObject} + {dragable.gameObject.name} ");

        currentDrag = dragable;
        currentAttachable = attachable;

        IAttacher attacher = (attachable == null ? null : attachable.GetCurrentAttached());
        if (attacher != null)
            attacher.OnDetach();

        dragable.StartDrag();

        OnStartDrag?.Invoke(dragable, hit);

    }

    private void EndDrag(IDragable dragable, Vector3 point)
    {
        currentAttachable = null;
        currentDrag = null;

        dragable.EndDrag(point + dragable.GetEndDragYOffset() * Vector3.up);

        OnEndDrag?.Invoke(dragable);

        IAttachable attachable = dragable as IAttachable;
        if (attachable != null)
        {
            InventoryObjectData data = attachable.GetInventoryObjectData();

            if (data != null)
            {
                Game.UIHandler.InventoryAdder.MoveToInventory(data, Input.mousePosition);
                Destroy(dragable.gameObject);
            }
            else
            {
                Debug.LogWarning("Droppped Attachable unattached but no Inventory Data was found, could not be get moved back to inventory.");
            }

        }
    }

    private void Attach(IAttachable attachable, IAttacher attacher)
    {
        currentAttachable = null;
        currentDrag = null;

        attacher.OnAttach(attachable);
        attachable.Attach(attacher);

        OnEndDrag?.Invoke(attachable);
    }

    public void ForceEndDrag()
    {
        EndDrag(currentDrag, Vector3.zero);
    }
    private static Vector3 CalculateAnimatedPreviewPosition(Vector3 hit, IAttacher attacher)
    {
        return attacher.GetPreviewPosition(hit) + attacher.GetPreviewDirectionVector() * (1f + Mathf.Sin(Time.time * 2f) * 0.2f ) * Game.Settings.CurrentZoomLevel;
        //return attacher.GetPreviewPosition(hit) + Game.Settings.AttachPreviewOffset * (3.5f + Mathf.Sin(Time.time * 2f) * 0.5f) * Game.Settings.CurrentZoomLevel;
    }
}
