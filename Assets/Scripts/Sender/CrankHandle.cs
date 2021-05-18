using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChargeInput
{
    event Action OnStartChargeEvent;
    event Action OnEndChargeEvent;
}

public class CrankHandle : MonoBehaviour, IDragable, IHoverable, IChargeInput
{
    [SerializeField] Crank crank;
    [SerializeField] Collider handleCollider;
    [SerializeField] Transform cursorParent;

    bool isDragging = false;

    public bool IsDragging => isDragging;

    public bool IsNull => this == null;

    public event Action OnStartHoverEvent;
    public event Action OnEndHoverEvent;

    public event Action OnStartChargeEvent;
    public event Action OnEndChargeEvent;

    public void StartHover()
    {
        OnStartHoverEvent?.Invoke();
    }

    public void EndHover()
    {
        OnEndHoverEvent?.Invoke();
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
        isDragging = true;
        handleCollider.enabled = false;
        Game.UIHandler.CustomCursor.SetCursorForcedState(isForced: true);

        crank.ResetAngleBefore();
        crank.StartPlayerInput();

        OnStartChargeEvent?.Invoke();
        Debug.Log("Start Drag");
    }

    public void UpdateDragPosition(Vector3 point, Vector3 vector3, bool useCustomPivot)
    {
        Game.UIHandler.CustomCursor.ForceCursorPosition(cursorParent.position);
        Vector2 head = crank.transform.position.To2D();
        Vector2 target = point.To2D();

        float angle = (Mathf.Atan2(target.x - head.x, target.y - head.y) * Mathf.Rad2Deg); //-180 => 180
        Debug.DrawLine(new Vector3(head.x,0, head.y), new Vector3(target.x, 0, target.y), Color.white);
        Debug.DrawLine(Vector3.zero, new Vector3((float)Mathf.Sin(angle * Mathf.Deg2Rad), 0f, (float)Mathf.Cos(angle * Mathf.Deg2Rad)), Color.cyan);
        crank.TryTurnTo(angle - 90);
    }
    public void EndDrag(Vector3 position)
    {
        isDragging = false;
        handleCollider.enabled = true;
        Game.UIHandler.CustomCursor.SetCursorForcedState(isForced: false);

        crank.EndPlayerInput();

        OnEndChargeEvent?.Invoke();
        Debug.Log("End Drag");
    }
}
