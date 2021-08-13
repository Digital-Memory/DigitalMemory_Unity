using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverHandle : MonoBehaviour, IDragable
{
    [SerializeField] Lever lever;
    [SerializeField] AnimationCurve distanceToRotationCurve;
    [SerializeField] AnimationCurve rotationToDistanceCurve;
    [SerializeField] Collider handleCollider;
    [InfoBox("Depending on how zoomed in we are the distance traveled by the mouse is diffent, thereby this multiplier can be used to manually remap the difference.")]
    [SerializeField] float distanceMultiplier = 1;
    [SerializeField] float d;

    [TextArea] [SerializeField] string tooltipText;

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

    public void EndDrag(Vector3 position)
    {
        isDragging = false;
        handleCollider.enabled = true;
        lever.EndPlayerInput();
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
    public Vector3 GetRaycastPlaneLockDirection(Vector3 point)
    {
        return Vector3.up;
    }

    public void StartDrag()
    {
        startRotation = lever.transform.localRotation.eulerAngles.GetLongestAxis();
        if (startRotation > 180)
            startRotation -= 360;

        startPosition = Vector3.zero;

        isDragging = true;
        lever.StartPlayerInput();
        handleCollider.enabled = false;
    }

    //Need to Improve this at some point
    public void UpdateDragPositionAndRotation(Vector3 point, Vector3 vector3, bool useCustomPivot, Quaternion rotation)
    {
        if (startPosition == Vector3.zero)
            startPosition = point;

        float distance = (point - startPosition).InvertAxis(Vector3.forward).GetLongestAxis() * (1 / Game.Settings.CurrentZoomLevel) * distanceMultiplier;
        d = distance;
        //Debug.LogWarning("filter " + (point - startPosition).InvertAxis(Vector3.forward));

        float currentRotation = distance * 10;

        float startOffsetDistance = rotationToDistanceCurve.Evaluate(startRotation);
        float distanceWithStartOffset = startOffsetDistance + distance;
        Debug.Log(distanceWithStartOffset);
        //float currentRotation = distanceToRotationCurve.Evaluate( distanceWithStartOffset);

        DebugDraw.AnimationCurve(rotationToDistanceCurve, startRotation);

        float angle = startRotation - currentRotation;

        lever.Turn(angle);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.right);
    }

    public string GetTooltipText()
    {
        return tooltipText;
    }
}
