using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class FloatSender : InputSender
{
    private const int MIN_VALUE = 0, MAX_VALUE = 1;
    [Range(MIN_VALUE, MAX_VALUE)]
    [SerializeField] float startValue;
    [SerializeField] float factorOffset = 0;
    [SerializeField] float factor = 360f;

    [Foldout("Effects")] [Expandable] [SerializeField] Effect whileChangeEffect, cantChangeEffect, forcedEndDragEffect;

    public event System.Action<float> OnSendInputValue;
    public event System.Action<float> OnSendCallbackWithFactor;

    Crank attachedCrank;

#if UNITY_EDITOR
    bool __isGivingInput;
#endif

    float currentValue;
    public float CurrentValue { get => currentValue; }


    FloatLimiter[] limits;


    void Awake()
    {
        limits = GetComponents<FloatLimiter>();
    }

    private void Start()
    {
        currentValue = startValue;
        if (TrySendInput(currentValue))
        {
            CallOnSendInputEvents(currentValue);
            OnSendCallbackWithFactor?.Invoke(Factorize(currentValue));
        }
    }

    internal bool TryGiveInput(float degrees, bool isAbsolute = false)
    {

        float newValue = isAbsolute ? ((degrees + factorOffset) / factor) : currentValue + ((degrees + factorOffset) / factor);

        //Debug.LogWarning("Float Sender is receiving " + newValue);

        if (IsInsideInputRange(newValue) && !IsInsideLimiter(newValue, limits))
        {
            if (TrySendInput(newValue))
            {
                currentValue = newValue;
                CallOnSendInputEvents(newValue);
                OnSendCallbackWithFactor?.Invoke(Factorize(newValue));
                Game.EffectHandler.Play(whileChangeEffect, gameObject);

                return true;
            }
            else
            {

                Game.EffectHandler.Play(whileChangeEffect, gameObject);
                return false;
            }
        }
        else
        {
            if (newValue < MIN_VALUE && Mathf.Abs(newValue - MIN_VALUE) > (180f / factor) && newValue > MAX_VALUE && Mathf.Abs(newValue - MAX_VALUE) > (180f / factor))
            {
                Game.DragHandler.ForceEndDrag();
                Game.EffectHandler.Play(forcedEndDragEffect, gameObject);
            }

            return false;
        }
    }

    public float Factorize(float valueWithoutFactor)
    {
        return valueWithoutFactor * factor - factorOffset;
    }

    protected override void CallOnSendInputEvents(float value)
    {
        base.CallOnSendInputEvents(value);
        OnSendInputValue?.Invoke(value);
    }

    public void SendCallback(float progression)
    {
        currentValue = progression;
        OnSendCallbackWithFactor?.Invoke(progression * factor - factorOffset);
    }

    private bool IsInsideLimiter(float newValue, FloatLimiter[] limits)
    {
        if (limits != null)
        {
            foreach (FloatLimiter limit in limits)
            {
                if (limit.IsInside(newValue))
                    return true;
            }
        }

        return false;
    }

    private bool IsInsideInputRange(float newValue)
    {
        return newValue < MAX_VALUE && newValue > MIN_VALUE;
    }

#if UNITY_EDITOR

    void Update()
    {
        __isGivingInput = false;
    }

#endif

    private bool TrySendInput(float progress)
    {
#if UNITY_EDITOR
        __isGivingInput = true;
#endif

        return (input == null || input.Try(progress));

    }

    void OnDrawGizmos()
    {
        Vector3 center = transform.position + Vector3.up * 2 + Vector3.forward;

        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(center, Vector3.one * 1.5f);

        float progress = ((currentValue - MIN_VALUE) / (MAX_VALUE - MIN_VALUE));

        Gizmos.color = Color.Lerp(Color.red, Color.green, progress);
        Gizmos.DrawWireCube(center, Vector3.one * (0.5f + progress));

#if UNITY_EDITOR

        if (input != null)
        {
            Gizmos.color = __isGivingInput ? Color.yellow : Color.gray;
            Gizmos.DrawLine(transform.position, input.transform.position);
        }
#endif
    }
}
