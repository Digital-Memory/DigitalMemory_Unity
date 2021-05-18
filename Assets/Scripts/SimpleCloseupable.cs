using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCloseupable : MonoBehaviour, ICloseupable
{
    private bool isInCloseup = false;
    public bool IsInCloseup => isInCloseup;

    public event Action OnStartCloseupEvent;
    public event Action OnEndCloseupEvent;

    public Vector3 GetCustomGlobalOffset()
    {
        return Vector3.zero;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Quaternion GetRotation()
    {
        return transform.rotation;
    }

    public void OnStartCloseup()
    {
        isInCloseup = true;
        OnStartCloseupEvent?.Invoke();
    }
    public void OnEndCloseup()
    {
        isInCloseup = false;
        OnEndCloseupEvent?.Invoke();
    }

    public bool ShouldOffset()
    {
        return false;
    }

    public void UpdatePositionAndRotation(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
    }
}
