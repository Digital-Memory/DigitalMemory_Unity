using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ZoomIn : MonoBehaviour, IClickable, IHoverable
{
    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] AnimationCurve zoomOutCurve;
    Vector3 virtualCameraPosition;
    SphereCollider coll;
    Material desaturationMaterial;
    [ShowNonSerializedField] bool active = false;
    [ShowNonSerializedField] bool inFadeoutPreview = false;
    [ShowNonSerializedField] bool animate = false;
    float current, target;
    int direction;

    public event Action OnStartHoverEvent;
    public event Action OnEndHoverEvent;

    public bool IsNull => this == null;

    public void Click()
    {
        Zoom();
    }

    private void Zoom()
    {
        cinemachineVirtualCamera.Priority = 100;
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
        coll = GetComponent<SphereCollider>();
        virtualCameraPosition = cinemachineVirtualCamera.transform.position;
        desaturationMaterial = Game.Settings.DesaturationMaterial;
        Game.ZoomInHandler.ChangedZoomIn += OnChangeZoom;
    }

    private void OnDisable()
    {
        Game.ZoomInHandler.ChangedZoomIn -= OnChangeZoom;
    }

    private void OnChangeZoom(bool isZoomedIn) {
        coll.enabled = !isZoomedIn;

        if (!isZoomedIn)
        {
            active = false;
            cinemachineVirtualCamera.Priority = 10;
        }
    }

    public void StartHover()
    {
        StartCoroutine(HoverRoutine());
    }

    private IEnumerator HoverRoutine()
    {
        yield return new WaitForSeconds(0.5f);

        Camera cam = Game.CameraController.Camera;
        Vector2 p = cam.WorldToScreenPoint(transform.position);
        Vector2 p2 = cam.WorldToScreenPoint(transform.position + cam.transform.right * coll.radius * 2);
        float size = Vector2.Distance(p, p2);

        desaturationMaterial.SetInt("mask", 1);
        desaturationMaterial.SetVector("pos", new Vector4(p.x, p.y, 0, 0));
        desaturationMaterial.SetFloat("size", size);
    }

    public void EndHover()
    {
        StopAllCoroutines();
        desaturationMaterial.SetInt("mask", 0);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
