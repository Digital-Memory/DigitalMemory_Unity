using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using NaughtyAttributes;

public class CinemachineCameraSwitcher : MonoBehaviour
{
    [Dropdown("GetCinemachineCameras")]
    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;

	private DropdownList<CinemachineVirtualCamera> GetCinemachineCameras()
	{
        return DropdownMonobehaviourList<CinemachineVirtualCamera>.FromObjectsOfType(FindObjectsOfType<CinemachineVirtualCamera>());
	}
	private void Start()
    {
        cinemachineVirtualCamera.MoveToTopOfPrioritySubqueue();
    }
}
