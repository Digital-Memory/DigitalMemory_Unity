using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ZoomIn : MonoBehaviour, IClickable
{
    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] AnimationCurve zoomOutCurve;
    Vector3 virtualCameraPosition;
    Collider coll;
    [ShowNonSerializedField] bool active = false;
    [ShowNonSerializedField] bool inFadeoutPreview = false;
    [ShowNonSerializedField] bool animate = false;
    float current, target;
    int direction;

    public void Click()
    {
        Zoom();
    }

    private void Zoom()
    {
        cinemachineVirtualCamera.MoveToTopOfPrioritySubqueue();
        active = true;
        Game.ZoomInHandler.ZoomIn(this);
    }

    public void TryChangeFadeoutPreview(bool showPreview) {
        if (showPreview != inFadeoutPreview)
        {
            inFadeoutPreview = showPreview;
            animate = true;
            target = showPreview ? zoomOutCurve[zoomOutCurve.length - 1].time : zoomOutCurve[0].time;
            direction = showPreview ? 1 : -1;
        }
    }

    private void Update()
    {
        if (animate) {
            current += direction * Time.deltaTime;

            if (Mathf.Abs(current - target) < Time.deltaTime * 2)
            {
                current = target;
                animate = false;
            }

            float factor = zoomOutCurve.Evaluate(current);
            Vector3 pos = virtualCameraPosition - cinemachineVirtualCamera.transform.forward * factor;
            cinemachineVirtualCamera.transform.position = pos;
        }
    }

    public bool IsClickable()
    {
        return !Game.ZoomInHandler.IsZoomedIn;
    }

    private void OnEnable()
    {
        coll = GetComponent<Collider>();
        virtualCameraPosition = cinemachineVirtualCamera.transform.position;
        Game.ZoomInHandler.ChangedZoomIn += OnChangeZoom;
    }

    private void OnDisable()
    {
        Game.ZoomInHandler.ChangedZoomIn -= OnChangeZoom;
    }

    private void OnChangeZoom(bool isZoomedIn) {
        coll.enabled = !isZoomedIn;

        if (!isZoomedIn)
            active = false;
    }
}
