using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using NaughtyAttributes;
using UnityEngine;

public class ZoomInHandler : Singleton<ZoomInHandler>
{
    CinemachineVirtualCamera overview;
    private ZoomIn current;
    public bool IsZoomedIn { get; internal set; }
    public System.Action<bool> ChangedZoomIn;

    [Expandable] [SerializeField] Effect zoomIn, zoomOut;


    private void OnEnable()
    {
        Game.Settings.CurrentZoomLevel = 1f;
    }

    protected override void Start()
    {
        base.Start();
    }
    public void ZoomIn(ZoomIn zoom)
    {
        Debug.Log($"Zoom in on: {zoom.name}");

        WebCommunicator.ZoomIn(zoom.WebData);
        Game.EffectHandler.Play(zoomIn, gameObject);
        Game.Settings.CurrentZoomLevel = 0.2f;

        current = zoom;
        IsZoomedIn = true;
        ChangedZoomIn?.Invoke(true);
    }

    private IEnumerator ZoomOut()
    {
        if (current != null)
        {
            Debug.Log($"Zoom out from: {current.name}");
            WebCommunicator.ZoomOut(current.WebData);
        } else
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

        yield return new WaitForSeconds(0.5f);

        IsZoomedIn = false;
    }

    [Button]
    internal void ForceZoomOut()
    {
        StartCoroutine(ZoomOut());
    }

    internal void RegisterAsOverview(CinemachineVirtualCamera cinemachineVirtualCamera)
    {
        overview = cinemachineVirtualCamera;
    }

    private void Update()
    {
        if (current == null || Game.UIHandler.EventSystem.IsPointerOverGameObject() || !current.DoesAllowZoomOut)
            return;

        var x = Input.mousePosition.x / Mathf.Max(1,(float)Screen.width);
        var y = Input.mousePosition.y / Mathf.Max(1,(float)Screen.height);

        float factor = Mathf.Max(x < 0.5f ? 1 - x : x, y < 0.5f ? 1 - y : y);
        current.TryChangeFadeoutPreview(factor > 0.9f);
        if (Input.GetMouseButtonDown(0) && factor > 0.9f)
        {
            StartCoroutine(ZoomOut());
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
