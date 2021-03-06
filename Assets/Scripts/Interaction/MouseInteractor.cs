using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInteractor : Singleton<MouseInteractor>
{
    [SerializeField] LayerMask draggingLayerMask, notDraggingLayerMask;
    [SerializeField] Effect onHoverDragableEnter, onHoverDragableExit;
    [SerializeField] float defaultMaxRaycastDistance;

    GameObject currenHoverTEMP;

    private bool raycastDistanceIsLocked = false;
    private Plane lockedRaycastPlane;
    private const float FLOOR_RAYCAST_DISTANCE = 15f;
    public bool InteractionIsBlocked { get; set; }


    private void OnEnable()
    {
        Game.DragHandler.OnStartDrag += LockRaycastDistance;
        Game.DragHandler.OnEndDrag += UnlockRaycastDistance;
    }

    private void OnDisable()
    {
        Game.DragHandler.OnStartDrag -= LockRaycastDistance;
        Game.DragHandler.OnEndDrag -= UnlockRaycastDistance;
    }

    private void Update()
    {
        if (InteractionIsBlocked)
            return;


        RaycastHit hit;
        Ray ray = Game.CameraController.Camera.ScreenPointToRay(Input.mousePosition);

        float lockedRaycastDistance = 0;
        if (raycastDistanceIsLocked)
            lockedRaycastPlane.Raycast(ray, out lockedRaycastDistance);

        LayerMask layerMask = Game.DragHandler.IsDragging ? draggingLayerMask : notDraggingLayerMask;

        if (!Physics.Raycast(ray, out hit, raycastDistanceIsLocked ? lockedRaycastDistance : defaultMaxRaycastDistance, layerMask))
        {
            hit.point = ray.GetPoint(raycastDistanceIsLocked ? lockedRaycastDistance : FLOOR_RAYCAST_DISTANCE);
        }

        Debug.DrawLine(hit.point + Vector3.right + Vector3.forward, hit.point + Vector3.left + Vector3.back, Color.magenta);
        Debug.DrawLine(hit.point + Vector3.left + Vector3.forward, hit.point + Vector3.right + Vector3.back, Color.magenta);

        if (Game.CloseupHandler.IsInCloseup)
        {
            Game.CloseupHandler.UpdateCloseup(hit, Input.GetMouseButtonDown(1));
        }
        else
        {

            if (Game.DragHandler.IsDragging)
            {
                Game.DragHandler.UpdateDrag(hit, ray);
            }
            else
            {
                UpdateNonDrag(hit);
            }
        }
    }

    private void UpdateNonDrag(RaycastHit hit)
    {
        IDragable dragable = null;
        IClickable clickable = null;
        IAttachable attachable = null;

        if (hit.collider != null)
        {
            dragable = hit.collider.GetComponentInParent<IDragable>();
            clickable = hit.collider.GetComponent<IClickable>();
            attachable = hit.collider.GetComponentInParent<IAttachable>();
        }

        Game.HoverHandler.UpdateHover(hit, dragable);

        //Mouse
        if (Input.GetMouseButtonDown(0))
        {
            if (dragable != null && dragable.IsDragable() && (clickable == null || clickable.ToString() == dragable.ToString()))
                Game.DragHandler.StartDrag(hit, dragable, attachable, Game.Settings.FallbackDragDistance);
            else if (clickable != null && clickable.IsClickable())
                ClickOn(clickable);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            ICloseupable closeupable = hit.collider.GetComponent<ICloseupable>();
            if (closeupable != null && !closeupable.IsInCloseup)
            {
                Game.CloseupHandler.StartCloseup(closeupable);
            }
        }
    }

    private void ClickOn(IClickable clickable)
    {
        clickable.Click();
    }

    private void LockRaycastDistance(IDragable dragable, RaycastHit hit)
    {
        if (dragable.ShouldLockOnDrag())
        {
            raycastDistanceIsLocked = true;
            lockedRaycastPlane = new Plane(dragable.GetRaycastPlaneLockDirection(hit.point), hit.point);
            DebugDraw.Plane(hit.point, lockedRaycastPlane.normal, 24, 6);
        }
    }
    private void UnlockRaycastDistance(IDragable dragable)
    {
        raycastDistanceIsLocked = false;
    }
}
