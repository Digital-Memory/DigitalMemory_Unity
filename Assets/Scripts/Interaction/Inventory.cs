using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, IAttacher
{
    [SerializeField] int contentCount, contentCountMax;
    [SerializeField] List<Transform> inventorySlotTransforms;
    [SerializeField] Collider collider;
    [SerializeField] Transform inventoryParent;

    public event Action OnStartHoverEvent;
    public event Action OnEndHoverEvent;

    private void OnEnable()
    {
        Game.DragHandler.OnStartDrag += OnStartDragAny;
        Game.DragHandler.OnEndDrag += OnEndDragAny;
    }

    private void OnDisable()
    {
        Game.DragHandler.OnStartDrag += OnStartDragAny;
        Game.DragHandler.OnEndDrag += OnEndDragAny;
    }

    private void OnEndDragAny()
    {
        collider.enabled = false;
    }

    private void OnStartDragAny(IDragable dragging, RaycastHit hit)
    {
        collider.enabled = true;
    }

    public bool CanAttach(string attachBehaviour)
    {
        return contentCount < contentCountMax;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void OnAttach(IAttachable attachable)
    {

    }

    public void OnDetach()
    {
        throw new System.NotImplementedException();
    }

    public Vector3 GetPreviewPosition(Vector3 point)
    {
        return GetPointOnPlane(point, CreatePlane(Game.CameraController.transform, inventorySlotTransforms[0].position));
    }

    private Vector3 GetPointOnPlane(Vector3 point, Plane plane)
    {
        Transform cameraTransform = Game.CameraController.transform;

        Ray ray = new Ray(point, point - cameraTransform.position);
        if (plane.Raycast(ray, out float enter))
        {
            return ray.GetPoint(enter);
        }

        return point;
    }

    private Plane CreatePlane(Transform cameraTransform, Vector3 pos)
    {
        Plane inventoryPlane = new Plane(cameraTransform.forward, pos);
        DebugDraw.Plane(pos, inventoryPlane.normal);
        return inventoryPlane;
    }

    public bool ResetPositionOnAttach()
    {
        return false;
    }

    public bool ResetOrientationOnAttach()
    {
        return true;
    }

    public Vector3 GetAttachOffset()
    {
        Vector3 point = GetPointOnPlane(Camera.main.ScreenPointToRay(Input.mousePosition).origin, CreatePlane(Game.CameraController.transform, inventorySlotTransforms[2].position));
        Debug.LogWarning("point " + point);
        return point;
    }

    public void HandleTransformOnAttach(Transform transformAttached)
    {
        transformAttached.parent = inventoryParent;
        Debug.DrawLine(inventorySlotTransforms[2].position, GetAttachOffset(), color: Color.red, 10f);
        transformAttached.position = GetAttachOffset();
        transformAttached.gameObject.layer = gameObject.layer;
        transformAttached.localScale = Vector3.one * 0.05f;
    }

    public void EndHover()
    {
        throw new NotImplementedException();
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
