using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ZoomInHandler : Singleton<ZoomInHandler>
{
    CinemachineVirtualCamera overview;
    private ZoomIn current;
    public bool IsZoomedIn { get; internal set; }
    public System.Action<bool> ChangedZoomIn;

    public void ZoomIn(ZoomIn zoom) {
        Debug.Log($"Zoom in on: {zoom.name}");
        WebCommunicator.ZoomIn(zoom.Id);
        current = zoom;
        IsZoomedIn = true;
        ChangedZoomIn?.Invoke(true);
    }

    private IEnumerator ZoomOut() {
        current = null;
        WebCommunicator.ZoomOut();

        if (overview == null)
            Debug.LogError("No overview found. Make sure you have and active ZoomOverview script with a virtual camera present.");

        ChangedZoomIn?.Invoke(false);

        yield return new WaitForSeconds(0.5f);

        IsZoomedIn = false;
    }

    internal void RegisterAsOverview(CinemachineVirtualCamera cinemachineVirtualCamera)
    {
        overview = cinemachineVirtualCamera;
    }

    private void Update()
    {
        if (current == null)
            return;

        var x = Input.mousePosition.x / (float)Screen.width;
        var y = Input.mousePosition.y / (float)Screen.height;

        float factor = Mathf.Max(x < 0.5f ? 1 - x : x, y < 0.5f ? 1 - y : y);
        current.TryChangeFadeoutPreview(factor > 0.9f);
        if (Input.GetMouseButtonDown(0) && factor > 0.9f) {
            StartCoroutine(ZoomOut());
        }

        //Debug.Log($"cam: {FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera.Name} with prio {FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera.Priority}");
    }

    //Need to Improve this at some point
    //Commented it out for now
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
