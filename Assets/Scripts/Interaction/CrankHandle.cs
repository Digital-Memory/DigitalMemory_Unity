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
        //crank.TrySnapToClosestDot();
    }

    public float GetEndDragYOffset()
    {
        return 0f;
    }

    public bool IsDragable()
    {
        return true;
    }

    public void StartDrag()
    {
        isDragging = true;
        handleCollider.enabled = false;
    }

    public void UpdateDragPosition(Vector3 point, Vector3 vector3)
    {
        Vector2 head = crank.transform.position.To2D();
        Vector2 target = point.To2D();

        float angle = (Mathf.Atan2((target.x - head.x) / 2, target.y - head.y) * Mathf.Rad2Deg); //-180 => 180
        crank.Turn(angle);
    }
}
