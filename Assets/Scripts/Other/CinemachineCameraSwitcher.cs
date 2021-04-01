using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using NaughtyAttributes;

public class CinemachineCameraSwitcher : ConditionedObject
{
    [Dropdown("GetCinemachineCameras")]
    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] bool isDefault = false;

	private DropdownList<CinemachineVirtualCamera> GetCinemachineCameras()
	{
        return DropdownMonobehaviourList<CinemachineVirtualCamera>.FromObjectsOfType(FindObjectsOfType<CinemachineVirtualCamera>());
	}

    private void Start()
    {
        if (isDefault)
            Switch();
    }

    [Button]
	private void Switch()
    {
        cinemachineVirtualCamera.MoveToTopOfPrioritySubqueue();
    }

    public override bool Try()
    {
        if (base.Try())
        {
            Switch();
            return true;
        }

        return false;
    }
    public override bool Try(bool on)
    {
        return Try();
    }

    public override bool Try(float value)
    {
        return Try();
    }
}
