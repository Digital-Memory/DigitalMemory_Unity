using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using NaughtyAttributes;
using UnityEngine;

public enum ZoomInState
{
    ZoomedOut,
    ZoomingOut,
    ZoomedIn,
    ZoomingIn,
}

public class ZoomInHandler : Singleton<ZoomInHandler>
{
    CinemachineVirtualCamera overview;
    private ZoomIn current;
    public ZoomInState zoomInState = ZoomInState.ZoomedOut;
    public System.Action<bool> ChangedZoomIn;

    [Expandable] [SerializeField] Effect zoomIn, zoomOut;


    private void OnEnable()
    {
        Game.Settings.CurrentZoomLevel = 1f;
    }

    protected override void Start()
    {
        Game.Settings.DesaturationMaterial.SetInt("mask", 0);
        base.Start();
    }

    protected virtual void OnDestroy()
    {
        Game.Settings.DesaturationMaterial.SetInt("mask", 0);
        Game.Settings.DesaturationMaterial.SetVector("pos", Vector4.zero);
        Game.Settings.DesaturationMaterial.SetFloat("size", 0);
    }


    public void TryZoomIn(ZoomIn zoom)
    {

        if (zoomInState != ZoomInState.ZoomedOut)
        {
            Debug.LogWarning($"Tried zooming in on {zoom.name} but state was {zoomInState}.");
            return;
        }

        StopAllCoroutines();
        StartCoroutine(ZoomInRoutine(zoom));
    }
    private void TryZoomOut()
    {
        if (zoomInState != ZoomInState.ZoomedIn)
            return;

        StopAllCoroutines();
        StartCoroutine(ZoomOutRoutine());
    }

    private IEnumerator ZoomInRoutine(ZoomIn zoom)
    {
        WebCommunicator.ZoomIn(zoom.WebData);
        Game.EffectHandler.Play(zoomIn, gameObject);
        Game.Settings.CurrentZoomLevel = 0.2f;

        current = zoom;
        zoomInState = ZoomInState.ZoomingIn;
        Debug.LogWarning($"Zoom in on: {current.name}");
        ChangedZoomIn?.Invoke(true);
        Game.MouseInteractor.InteractionIsBlocked = true;

        yield return new WaitForSeconds(1f);

        Game.MouseInteractor.InteractionIsBlocked = false;
        zoomInState = ZoomInState.ZoomedIn;
    }

    private IEnumerator ZoomOutRoutine()
    {
        if (current != null)
        {
            Debug.Log($"Zoom out from: {current.name}");
            WebCommunicator.ZoomOut(current.WebData);
        }
        else
        {
            Debug.LogWarning("Zoom Out was called even though the previous zoom in was undefined.");
        }
        Game.VideoPlayerHandler.Pause();
        Game.EffectHandler.Play(zoomOut, gameObject);
        Game.Settings.CurrentZoomLevel = 1f;

        current = null;

        if (overview == null)
            Debug.LogError("No overview found. Make sure you have and active ZoomOverview script with a virtual camera present.");

        ChangedZoomIn?.Invoke(false);

        //Hotfix but works
        overview.Priority = 50;
        zoomInState = ZoomInState.ZoomingOut;
        Game.MouseInteractor.InteractionIsBlocked = true;

        yield return new WaitForSeconds(1f);

        Game.MouseInteractor.InteractionIsBlocked = false;
        zoomInState = ZoomInState.ZoomedOut;
    }

    [Button]
    internal void ForceZoomOut()
    {
        StopAllCoroutines();
        StartCoroutine(ZoomOutRoutine());
    }

    internal void RegisterAsOverview(CinemachineVirtualCamera cinemachineVirtualCamera)
    {
        overview = cinemachineVirtualCamera;
    }

    private void Update()
    {
        if (current == null || Game.UIHandler.EventSystem.IsPointerOverGameObject() || !current.DoesAllowZoomOut)
            return;

        var x = Input.mousePosition.x / Mathf.Max(1, (float)Screen.width);
        var y = Input.mousePosition.y / Mathf.Max(1, (float)Screen.height);

        float factor = Mathf.Max(x < 0.5f ? 1 - x : x, y < 0.5f ? 1 - y : y);
        current.TryChangeFadeoutPreview(factor > 0.9f);
        if (Input.GetMouseButtonDown(0) && factor > 0.9f)
        {
            TryZoomOut();
        }

        //Debug.Log($"cam: {FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera.Name} with prio {FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera.Priority}");
    }

    //Need to Improve this at some point
    //private void OnGUI()
    //{
    //    string str = "";
    //    foreach (CinemachineVirtualCamera vcam in FindObjectsOfType<CinemachineVirtualCamera>())
    //    {
    //        str += (vcam == (FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera) ? ">>> " : "");
    //        str += vcam.gameObject.transform.parent ? vcam.gameObject.transform.parent.name : vcam.gameObject.name;
    //        str += $" - {vcam.Name } - {vcam.Priority.ToString() }\n";
    //    }
    //    GUILayout.Box(str, GUILayout.Width(450));
    //}
}
