using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class ZoomOverview : MonoBehaviour
{
    CinemachineVirtualCamera cinemachineVirtualCamera;

    private void OnEnable()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.Priority = 51;
        Game.ZoomInHandler.RegisterAsOverview(cinemachineVirtualCamera);
    }
}
