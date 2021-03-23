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

    public void EndDrag(Vector3 position)
    {
        isDragging = false;
        handleCollider.enabled = true;
        isSnapping = true;
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

    private void Update()
    {
        if (isSnapping && !isDragging)
        {
            float current = lever.GetRotation().eulerAngles.z;
            Vector2 minMax = lever.GetMinMaxRotations();

            float deltaMin = Mathf.Abs(Mathf.DeltaAngle(current, minMax.x));
            float deltaMax = Mathf.Abs(Mathf.DeltaAngle(current, minMax.y));

            if (deltaMax <= 1f || deltaMin <= 1f)
            {
                Debug.Log("target reached... min:" + deltaMin + " max:"+deltaMax);
                isSnapping = false;
            }
            else
            {
                bool minIsCloser = (deltaMin < deltaMax);
                Debug.Log("lerping to " + (minIsCloser ? "min" : "max"));
                lever.Turn(Mathf.LerpAngle(current, minIsCloser ? minMax.x : minMax.y, Time.deltaTime * snapSpeed));
                //lever.Turn(minIsCloser ? -1f : 1f);
            }
        }
    }
}
