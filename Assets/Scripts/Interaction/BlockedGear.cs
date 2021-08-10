using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockedGear : MonoBehaviour, IClickable
{
    bool blocked = true;
    public event Action OnFreeGear;
    public event Action OnClickEvent;

    [SerializeField] AnimationCurve xRotationIfBlocked, xRotationIfFree;

    [SerializeField] Collider clickColliderToDisableObFree;

    public void Click()
    {
        blocked = false;
        OnFreeGear?.Invoke();
        OnClickEvent?.Invoke();
        clickColliderToDisableObFree.enabled = false;
    }

    public bool IsClickable()
    {
        return blocked;
    }

    private void Update()
    {
        transform.localRotation = Quaternion.Euler(0,(blocked ? xRotationIfBlocked : xRotationIfFree).Evaluate(Time.time), 0);
    }
}
