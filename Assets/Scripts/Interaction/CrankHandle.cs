using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankHandle : MonoBehaviour, IDragable
{
    [SerializeField] Crank crank;

    [SerializeField] Collider handleCollider;

    bool isDragging = false;

    public bool IsDragging => isDragging;

    public void EndDrag(Vector3 position)
    {
        isDragging = false;
        handleCollider.enabled = true;
        Debug.Log("End Drag");
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
        handleCollider.enabled = false;
        Debug.Log("Start Drag");
    }

    public void UpdateDragPosition(Vector3 point, Vector3 vector3)
    {
        Vector2 head = crank.transform.position.To2D();
        Vector2 target = point.To2D();

        float angle = (Mathf.Atan2((target.x - head.x) / 2, target.y - head.y) * Mathf.Rad2Deg); //-180 => 180
        Debug.DrawLine(head, target, Color.white);
        Debug.DrawLine(Vector3.zero, new Vector2((float)Mathf.Cos(angle), (float)Mathf.Sin(angle)), Color.red);
        crank.Turn(angle);
    }
}
