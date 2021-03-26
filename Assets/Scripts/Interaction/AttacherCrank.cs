using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class AttacherCrank : Attacher, IInputSender
{
    [SerializeField] float startValue, minValue, maxValue;
    [SerializeField] float factor = 360f;
    [SerializeField] InputObject input;

    [SerializeField] Effect tickEffect;

    public event System.Action<float> OnChangeValue;
    public event Action OnSendInput;

#if UNITY_EDITOR
    bool __isGivingInput;
#endif

    float currentValue;

    void Awake()
    {
        attachmentName = "Crank";
        Start();
    }

    internal bool TryRotate(float degrees)
    {
        float newValue = currentValue + (degrees / factor);

        //Debug.Log("change value: " + degrees/factor + "New value: " + newValue + " min: "+ minValue + " max: " + maxValue);

        if (newValue < maxValue && newValue > minValue)
        {
            if (TryGiveInput(((currentValue - minValue) / (maxValue - minValue))))
            {
                currentValue = newValue;
                OnChangeValue?.Invoke(newValue);
                OnSendInput?.Invoke();

                Game.EffectHandler.Play(tickEffect, gameObject);

                return true;
            } else
            {
                return false;
            }
        }
        else
        {
            if (newValue < minValue && Mathf.Abs(newValue - minValue) > (180f / factor)
                && newValue > maxValue && Mathf.Abs(newValue - maxValue) > (180f / factor))
                Game.MouseInteractor.ForceEndDrag();

            return false;
        }
    }

#if UNITY_EDITOR

    void Update()
    {
        __isGivingInput = false;
    }

#endif

    private bool TryGiveInput(float progress)
    {
#if UNITY_EDITOR
        __isGivingInput = true;
#endif

        return (input != null && input.Try(progress));

    }

    void OnDrawGizmos()
    {
        Vector3 center = transform.position + Vector3.up * 2 + Vector3.forward;

        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(center, Vector3.one * 1.5f);

        float progress = ((currentValue - minValue) / (maxValue - minValue));

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
