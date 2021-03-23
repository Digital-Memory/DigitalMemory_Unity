using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class AttacherLever : Attacher
{
    public bool isTop;
    public float topRotation, bottomRotation;
    public event System.Action<bool> OnLeverTurn;

    [SerializeField] Effect onSwitchEffect;

    void Awake()
    {
        attachmentName = "Lever";
    }

    internal Quaternion GetLeverRotationForCurrentPosition()
    {
        return isTop ? Quaternion.Euler(0, 0, topRotation) : Quaternion.Euler(0, 0, bottomRotation);
    }

    internal Quaternion Switch(float angle)
    {
        Game.EffectHandler.Play(onSwitchEffect,gameObject);

        isTop = !isTop;
        OnLeverTurn(isTop);
        return GetLeverRotationForCurrentPosition();
    }
}
