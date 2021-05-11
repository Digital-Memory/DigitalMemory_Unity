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
        Game.ZoomInHandler.RegisterAsOverview(cinemachineVirtualCamera);
    }
}
