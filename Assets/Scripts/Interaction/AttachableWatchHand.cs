using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AttachableWatchHand : MonoBehaviour, IDragable
{
    [SerializeField] AttachableWatch watch;

    [SerializeField] Transform pin;
    [SerializeField] Vector3 pinLocalPositionIn, pinLocalPositionOut;

    float targetTime;
    float timeBefore;

    bool moves = false;
    bool isDragging = false;

    public bool IsDragging => isDragging;

    public bool IsNull => this == null;

    public event Action OnStartHoverEvent;
    public event Action OnEndHoverEvent;
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
        pin.transform.localPosition = pinLocalPositionIn;
        watch.TrySnapToClosestDot();
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

    public void StartDrag()
    {
        isDragging = true;
        pin.transform.localPosition = pinLocalPositionOut;
    }

    public void UpdateDragPosition(Vector3 point, Vector3 vector3, bool useCustomPivot)
    {
        Vector2 head = transform.position.To2D();
        Vector2 target = point.To2D();

        float angle = Mathf.Atan2((target.x - head.x) / 2, target.y - head.y) * Mathf.Rad2Deg; //-180 => 180
        targetTime = ((angle + 90f) / 30f + 12f) % 12f; //0 => 1;

        if (Mathf.Abs((targetTime) - (timeBefore)) < 6f)
        {
            watch.UpdateWatchTimeByHand(targetTime);
            timeBefore = targetTime;
        }
    }
}
