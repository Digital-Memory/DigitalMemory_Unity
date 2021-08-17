using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ZoomIn : InputObject, IClickable, IHoverable
{
    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] AnimationCurve zoomOutCurve;
    Vector3 virtualCameraPosition;
    SphereCollider coll;
    Material desaturationMaterial;
    [ShowNonSerializedField] bool inFadeoutPreview = false;
    [ShowNonSerializedField] bool animate = false;

    internal void OverrideVirtualCamera(CinemachineVirtualCamera camera)
    {
        cinemachineVirtualCamera = camera;
        virtualCameraPosition = cinemachineVirtualCamera.transform.position;
    }

    [TextArea] [SerializeField] string tooltipText;
    float current, target;
    int direction;

    public event Action OnStartHoverEvent;
    public event Action OnEndHoverEvent;
    public event Action OnClickEvent;

    [SerializeField] List<InputObject> InputObjects = new List<InputObject>();
    [SerializeField] private bool isStartingPoint;

    [SerializeField] ActionOnInput actionOnInput;
    [ShowIf("actionOnInputIsZoomInOnOther")]
    [SerializeField] ZoomIn otherZoomIn;

    bool actionOnInputIsZoomInOnOther => actionOnInput == ActionOnInput.ZoomInOnOther;


    public override bool Try()
    {
        if (actionOnInput == ActionOnInput.ZoomOut)
        {
            Debug.Log($"ZoomOut Impulse send to {name}.");
            Game.ZoomInHandler.ForceZoomOut();
        }
        else
        {
            otherZoomIn.DoZoomIn();
        }

        return true;
    }


    public bool IsNull => this == null;

    public int Id { get; internal set; }

    [SerializeField] private bool doesAllowZoomOut = true;
    [SerializeField] private bool blockZoomInOnClick = false;
    public bool DoesAllowZoomOut => doesAllowZoomOut;

    public string WebData = "";

    private void Awake()
    {
        if (isStartingPoint)
            DoZoomIn();
    }

    public void Click()
    {
        DoZoomIn();
        OnClickEvent?.Invoke();
    }

    public void DoZoomIn()
    {
        cinemachineVirtualCamera.Priority = 100;
        Game.ZoomInHandler.TryZoomIn(this);
        SendInputObjects(true);
    }

    private void DoZoomOut()
    {
        Debug.Log($"{name} set prio to 10.");
        cinemachineVirtualCamera.Priority = 10;
        SendInputObjects(false);
    }
    private void SendInputObjects(bool b)
    {
        if (InputObjects != null)
        {
            foreach (InputObject inputObject in InputObjects)
            {
                if (inputObject != null)
                    inputObject.Try(b);
            }
        }
    }

    public void TryChangeFadeoutPreview(bool showPreview)
    {
        if (showPreview == inFadeoutPreview)
            return;

        inFadeoutPreview = showPreview;
        animate = true;
        target = showPreview ? zoomOutCurve[zoomOutCurve.length - 1].time : zoomOutCurve[0].time;
        direction = showPreview ? 1 : -1;
    }

    private void Update()
    {
        if (animate)
        {
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
        return Game.ZoomInHandler.zoomInState == ZoomInState.ZoomedOut;
    }

    private void OnEnable()
    {
        coll = GetComponent<SphereCollider>();

        if (blockZoomInOnClick)
            coll.enabled = false;

        virtualCameraPosition = cinemachineVirtualCamera.transform.position;
        desaturationMaterial = Game.Settings.DesaturationMaterial;

        Game.ZoomInHandler.ChangedZoomIn += OnChangeZoom;
    }

    private void OnDisable()
    {
        Game.ZoomInHandler.ChangedZoomIn -= OnChangeZoom;
    }

    private void OnChangeZoom(bool isZoomedIn)
    {
        Debug.LogWarning($"{name} changed zoom in to {isZoomedIn}");

        if (!blockZoomInOnClick || isZoomedIn)
        {
            coll.enabled = !isZoomedIn;
            Debug.LogWarning($"{name} set collider {((!isZoomedIn) ? "enabled" : "disabled")}");
        }

        if (!isZoomedIn)
            DoZoomOut();
    }

    public void StartHover()
    {
        StartCoroutine(HoverRoutine());
        OnStartHoverEvent?.Invoke();
    }
    public void EndHover()
    {
        StopAllCoroutines();
        desaturationMaterial.SetInt("mask", 0);
        OnEndHoverEvent?.Invoke();
    }

    private IEnumerator HoverRoutine()
    {
        yield return new WaitForSeconds(0.2f);

        Camera cam = Game.CameraController.Camera;
        Vector2 p = cam.WorldToScreenPoint(transform.position);
        Vector2 p2 = cam.WorldToScreenPoint(transform.position + cam.transform.right * coll.radius * 2);
        float size = Vector2.Distance(p, p2);

        desaturationMaterial.SetInt("mask", 1);
        desaturationMaterial.SetVector("pos", new Vector4(p.x, p.y, 0, 0));
        desaturationMaterial.SetFloat("size", size);
    }

    private void OnDrawGizmosSelected()
    {
        if (InputObjects != null)
        {
            Gizmos.color = Color.yellow;
            foreach (InputObject inputObject in InputObjects)
            {
                if (inputObject != null)
                    Gizmos.DrawLine(inputObject.gameObject.transform.position, transform.position);
            }
        }
    }

    public string GetTooltipText()
    {
        return tooltipText;
    }

    private enum ActionOnInput
    {
        ZoomOut,
        ZoomInOnOther,
    }
}
