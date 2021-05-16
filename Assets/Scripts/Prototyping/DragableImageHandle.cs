using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class DragableImageHandle : MonoBehaviour, IDragable
{
    [Dropdown("VectorValues")]
    public Vector3 forward;
    private Vector3[] VectorValues = new Vector3[] { Vector3.right, Vector3.left, Vector3.forward, Vector3.back, Vector3.up, Vector3.down };

    [SerializeField] Transform imageChild;

    [SerializeField] Vector3 upperPositionOffset, lowerPositionOffset;
    float distanceBetweenUpperAndLower;

    Vector3 localPositionOnStartDrag;
    Vector3 clickPositonOnStartDrag;

    public bool IsNull => this == null;

    public event Action OnStartHoverEvent;
    public event Action OnEndHoverEvent;

    void Reset ()
    {
        imageChild = transform.GetChild(0);
        upperPositionOffset = Vector3.up;
        lowerPositionOffset = Vector3.down;

        if (imageChild != null)
        {
            SetCurrentChildPositionAsUpper();
            lowerPositionOffset = lowerPositionOffset + Vector3.down * 3f;
        }
    }

    [Button]
    private void SetCurrentChildPositionAsUpper ()
    {
        upperPositionOffset = imageChild.localPosition;
    }

    [Button]
    private void SetCurrentChildPositionAsLower()
    {
        lowerPositionOffset = imageChild.localPosition;
    }

    private void Start()
    {
        imageChild.localPosition = lowerPositionOffset;
        distanceBetweenUpperAndLower = Vector3.Distance(upperPositionOffset, lowerPositionOffset);
    }

    public void EndDrag(Vector3 position)
    {
    }

    public void EndHover()
    {
        OnEndHoverEvent?.Invoke();
    }

    public float GetEndDragYOffset()
    {
        return 0f;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
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
        return forward;
    }

    public void StartDrag()
    {
        localPositionOnStartDrag = imageChild.localPosition;
        clickPositonOnStartDrag = Vector3.zero;
    }

    public void StartHover()
    {
        OnStartHoverEvent?.Invoke();
    }

    public void UpdateDragPosition(Vector3 point, Vector3 vector3, bool useCustomPivot)
    {
        if (clickPositonOnStartDrag == Vector3.zero)
            clickPositonOnStartDrag = point;

        float dragDistance = (clickPositonOnStartDrag - point).FilterByAxis((upperPositionOffset - lowerPositionOffset).normalized);
        Vector3 lerp = (lowerPositionOffset - upperPositionOffset) * (dragDistance / distanceBetweenUpperAndLower);
        imageChild.localPosition = (localPositionOnStartDrag + (lerp)).Clamp(lowerPositionOffset, upperPositionOffset);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + upperPositionOffset, transform.position + upperPositionOffset + forward);
        Gizmos.DrawLine(transform.position + lowerPositionOffset, transform.position + lowerPositionOffset + forward);
        Gizmos.DrawLine(transform.position + upperPositionOffset + forward * 0.33f, transform.position + lowerPositionOffset + forward * 0.33f);
    }
}
