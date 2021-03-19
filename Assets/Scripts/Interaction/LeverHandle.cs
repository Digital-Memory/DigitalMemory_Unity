using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverHandle : MonoBehaviour, IDragable
{
    [SerializeField] Lever lever;
    [SerializeField] AnimationCurve distanceToRotationCurve;
    [SerializeField] Collider handleCollider;

    bool isDragging = false;

    float startRotation;
    Vector3 startPosition;

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
        startRotation = lever.GetRotation().eulerAngles.z;
        startPosition = Vector3.zero;

        isDragging = true;
        handleCollider.enabled = false;
    }

    public void UpdateDragPosition(Vector3 point, Vector3 vector3)
    {
        if (startPosition == Vector3.zero)
            startPosition = point;

        float angle = startRotation - distanceToRotationCurve.Evaluate(point.x - startPosition.x);
        lever.Turn(angle);
    }
}
