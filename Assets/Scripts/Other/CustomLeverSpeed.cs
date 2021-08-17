using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Attacher))]
public class CustomLeverSpeed : MonoBehaviour
{
    Attacher attacher;
    public float CustomSpeed = 1;

    private void OnEnable()
    {
        attacher = GetComponent<Attacher>();

        if (!TryAssignValue())
            attacher.OnChangeAttached += OnChangeAttached;
    }

    private void OnDisable()
    {
        attacher.OnChangeAttached -= OnChangeAttached;
    }

    private void OnChangeAttached(bool isAttached, string attachment)
    {
        if (isAttached)
        {
            TryAssignValue();
        }
    }

    private bool TryAssignValue()
    {
        LeverHandle handle = GetComponentInChildren<LeverHandle>();
        if (handle)
        {
            handle.SetCustomMouseDistanceMultiplier(CustomSpeed);
            return true;
        }

        return false;
    }
}
