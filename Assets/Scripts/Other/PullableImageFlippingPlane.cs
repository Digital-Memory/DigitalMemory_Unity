using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using TMPro;

public class PullableImageFlippingPlane : MonoBehaviour, IDragable, IPullableImageDisplay
{
    Quaternion localRotationOnStartDrag;
    Vector3 clickPositonOnStartDrag;

    Quaternion localRoationDefault;
    [SerializeField] Quaternion localRotationFlipped;
    [SerializeField] AnimationCurve distanceToRotationCurve = AnimationCurve.EaseInOut(0,0,30,500);
    [SerializeField] float d;

    [SerializeField] private bool showsImage = true;
    [SerializeField] bool rotateVertically = false;

    public bool IsNull => this == null;

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] TMP_Text tMP_Text;
    public MeshRenderer MeshRenderer => meshRenderer;

    public event Action OnStartHoverEvent;
    public event Action OnEndHoverEvent;

    [Foldout("Effects")] [Expandable] [SerializeField] Effect startUseEffect, endUseEffect;
    private void OnEnable()
    {
        localRoationDefault = transform.localRotation;
    }
    public void EndDrag(Vector3 position)
    {
        Game.EffectHandler.Play(endUseEffect, gameObject);
        StartCoroutine(PanToClosestFullRotation());
    }

    private IEnumerator PanToClosestFullRotation()
    {
        float distanceToImageFront = Quaternion.Angle(transform.localRotation, localRoationDefault);
        float distanceToTextFront = Quaternion.Angle(transform.localRotation, localRotationFlipped);
        Quaternion targetRotation = distanceToImageFront < distanceToTextFront ? localRoationDefault : localRotationFlipped;

        while (Quaternion.Angle(transform.localRotation, targetRotation) > 0.01f)
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, Time.deltaTime * 100);
            yield return null;
        }
    }

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
    public Vector3 GetRaycastPlaneLockDirection(Vector3 point)
    {
        return Game.CameraController.Camera.transform.forward;
        //return transform.parent.right;
    }

    public void StartDrag()
    {
        StopAllCoroutines();
        localRotationOnStartDrag = transform.localRotation;
        clickPositonOnStartDrag = Vector3.zero;
        Game.EffectHandler.Play(startUseEffect, gameObject);
    }


    public void UpdateDragPositionAndRotation(Vector3 point, Vector3 vector3, bool useCustomPivot, Quaternion rotation)
    {
        if (clickPositonOnStartDrag == Vector3.zero)
            clickPositonOnStartDrag = point;

        float dragDistance = (clickPositonOnStartDrag - point).GetLongestAxis();
        d = dragDistance;
        Quaternion lerp = DistanceToRotation(dragDistance);
        transform.localRotation = (localRotationOnStartDrag * lerp);
    }

    private Quaternion DistanceToRotation(float dragDistance)
    {
        float angle = distanceToRotationCurve.Evaluate(Mathf.Abs(dragDistance)) * Mathf.Sign(dragDistance);
        return Quaternion.Euler(new Vector3(angle, angle, angle).FilterByAxisVector(rotateVertically ? Vector3.up : Vector3.forward));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.parent.up * d);
    }

    public void DisplayText(string str)
    {
        tMP_Text.text = str;
    }
}

public interface IPullableImageDisplay
{
    MeshRenderer MeshRenderer { get; }
    void DisplayText(string str);
}