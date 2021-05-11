using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ZoomInHandler : Singleton<ZoomInHandler>
{
    CinemachineVirtualCamera overview;
    private ZoomIn current;
    public bool IsZoomedIn => current != null;
    public System.Action<bool> ChangedZoomIn;

    public void ZoomIn(ZoomIn zoom) {
        current = zoom;
        ChangedZoomIn?.Invoke(true);
    }

    public void ZoomOut() {
        current = null;

        if (overview == null)
            Debug.LogError("No overview found. Make sure you have and active ZoomOverview script with a virtual camera present.");
        else
            overview.MoveToTopOfPrioritySubqueue();

        ChangedZoomIn?.Invoke(false);
    }

    internal void RegisterAsOverview(CinemachineVirtualCamera cinemachineVirtualCamera)
    {
        overview = cinemachineVirtualCamera;
    }

    private void Update()
    {
        if (!IsZoomedIn)
            return;

        var x = Input.mousePosition.x / (float)Screen.width;
        var y = Input.mousePosition.y / (float)Screen.height;

        float factor = Mathf.Max(x < 0.5f ? 1 - x : x, y < 0.5f ? 1 - y : y);
        current.TryChangeFadeoutPreview(factor > 0.9f);
        if (Input.GetMouseButtonDown(0) && factor > 0.9f) {
            ZoomOut();
        }
    }
}
