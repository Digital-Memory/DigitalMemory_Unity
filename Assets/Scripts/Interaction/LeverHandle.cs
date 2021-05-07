using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverHandle : MonoBehaviour, IDragable
{
    [SerializeField] Lever lever;
    [SerializeField] AnimationCurve distanceToRotationCurve;
    [SerializeField] Collider handleCollider;

    [SerializeField] float snapSpeed = 10f;

    bool isSnapping = false;
    bool isDragging = false;

    float startRotation;
    Vector3 startPosition;

    public bool IsDragging => isDragging;

    public bool IsNull => this == null;

    public event System.Action OnStartHoverEvent;
    public event System.Action OnEndHoverEvent;

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

    public void EndDrag(Vector3 position)
    {
        isDragging = false;
        handleCollider.enabled = true;
        isSnapping = true;
        lever.TrySnap();
    }

    public float GetEndDragYOffset()
    {
        return 0f;
    }

    public bool IsDragable()
    {
        return true;
    }

    public bool ShouldLockOnDrag()
    {
        return true;
    }
    public Vector3 GetRaycastPlaneLockDirection()
    {
        return Vector3.up;
    }

    public void StartDrag()
    {
        startRotation = lever.transform.localRotation.eulerAngles.GetLongestAxis();
        startPosition = Vector3.zero;

        isDragging = true;
        lever.StopAnySnap();
        handleCollider.enabled = false;
    }

    public void UpdateDragPosition(Vector3 point, Vector3 vector3, bool useCustomPivot)
    {
        if (startPosition == Vector3.zero)
            startPosition = point;

        float distance = (point - startPosition).InvertAxis(Vector3.forward).GetLongestAxis() * (1 / Game.Settings.CurrentZoomLevel);

        float currentRotation = distance * 10;
        float angle = startRotation - currentRotation;

        //Debug.LogWarning(lever.transform.localRotation.eulerAngles + " => " + startRotation + " - (" + distance + " => )" + currentRotation + " = " + angle);

        lever.Turn(angle);
    }
}
