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

    [SerializeField] Transform rotatingWheel;

    [Foldout("Effects")] [Expandable] [SerializeField] Effect startUseEffect, endUseEffect, whileChangeEffect, cantChangeEffect, forcedEndDragEffect;
    [SerializeField] AudioClip whileChangeSound;

    public event System.Action<float> OnSendInputValue;
    public event System.Action<float> OnSendCallbackWithFactor;

    public event System.Action OnStartPlayerInput;
    public event System.Action OnEndPlayerInput;

    Crank attachedCrank;

#if UNITY_EDITOR
    bool __isGivingInput;
#endif

    float currentValue;
    public float CurrentValue { get => currentValue; }

    float valueAtWhichPlayedEffectLast;
    const float VALUE_DISTANCE_NEEDED_TO_PLAY_EFFECT = 0.1f;


    FloatLimiter[] limits;


    void Awake()
    {
        limits = GetComponents<FloatLimiter>();
    }

    private void Start()
    {
        currentValue = startValue;
        if (TrySendInput(currentValue))
            FactorizeAndSendInput(currentValue);
    }

    public bool TryGiveInput(float degrees, bool isAbsolute = false)
    {

        float rawValue = isAbsolute ? ((degrees + factorOffset) / factor) : currentValue + ((degrees + factorOffset) / factor);
        return TryGiveInputRaw(rawValue);
    }

    public bool TryGiveInputRaw(float rawValue)
    {
        if (!IsInsideInputRange(rawValue) || !IsInsideLimiter(rawValue, limits))
        {
            if (rawValue < MIN_VALUE && Mathf.Abs(rawValue - MIN_VALUE) > (180f / factor) && rawValue > MAX_VALUE && Mathf.Abs(rawValue - MAX_VALUE) > (180f / factor))
            {
                Game.DragHandler.ForceEndDrag();
                Game.EffectHandler.Play(forcedEndDragEffect, gameObject);
            }

            rawValue = Mathf.Clamp(rawValue, MIN_VALUE, MAX_VALUE);
        }

        if (TrySendInput(rawValue))
        {
            currentValue = rawValue;

            FactorizeAndSendInput(rawValue);
            TryPlayChangeEffectAt(rawValue);

            return true;
        }
        else
        {
            return false;
        }
    }

    internal void OverrideInputReference(InputObject inputObject)
    {
        input = inputObject;
    }

    private void TryPlayChangeEffectAt(float rawValue)
    {
        if (Mathf.Abs(rawValue - valueAtWhichPlayedEffectLast) > VALUE_DISTANCE_NEEDED_TO_PLAY_EFFECT)
        {
            valueAtWhichPlayedEffectLast = rawValue;
            Game.EffectHandler.Play(whileChangeEffect, gameObject);
            Game.SoundPlayer.Play(whileChangeSound, volume: 0.25f, pitch: 0.5f + rawValue);
        }
    }

    private void FactorizeAndSendInput(float rawValue)
    {
        float factorized = Factorize(rawValue);
        CallOnSendInputEvents(rawValue);
        OnSendCallbackWithFactor?.Invoke(factorized);

        if (rotatingWheel != null)
            rotatingWheel.transform.localRotation = Quaternion.Euler(0, factorized, 0);
    }

    public void StartPlayerInput()
    {
        Game.EffectHandler.Play(startUseEffect, gameObject);
        OnStartPlayerInput?.Invoke();
    }

    public void EndPlayerInput()
    {
        Game.EffectHandler.Play(endUseEffect, gameObject);
        OnEndPlayerInput?.Invoke();
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
                if (!limit.IsInside(newValue))
                    return false;
            }
        }

        return true;
    }

    private bool IsInsideInputRange(float newValue)
    {
        return newValue <= MAX_VALUE && newValue >= MIN_VALUE;
    }


    //Need to Improve this at some point
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
        bool canTryInput = (input == null || input.Try(progress));

        if (canTryInput)
            SendSecondaryInput(InputType.Float, floatValue: progress);

        return canTryInput;
    }

    void OnDrawGizmos()
    {
        Vector3 center = transform.position;

        Gizmos.color = Color.white;
        float progress = ((currentValue - MIN_VALUE) / (MAX_VALUE - MIN_VALUE));

        Gizmos.DrawLine(center + Vector3.forward * 5 * Game.Settings.CurrentZoomLevel * progress, center);


        Gizmos.color = Color.Lerp(Color.red, Color.green, progress);
        Gizmos.DrawLine(center + Vector3.forward * 5 * Game.Settings.CurrentZoomLevel, center + Vector3.forward * 5 * Game.Settings.CurrentZoomLevel * progress);

#if UNITY_EDITOR

        if (input != null)
        {
            Gizmos.color = __isGivingInput ? Color.yellow : Color.gray;
            Gizmos.DrawLine(transform.position, input.transform.position);
        }

        if (hasSecondaryInput && secondary != null && secondary.Length > 0)
        {
            foreach (InputObject inputObject in secondary)
            {
                if (inputObject == null)
                    Debug.LogError($"secondary input list contains empty object ({name})");

                Gizmos.DrawLine(transform.position, inputObject.transform.position);
            }
        }
#endif
    }
}
