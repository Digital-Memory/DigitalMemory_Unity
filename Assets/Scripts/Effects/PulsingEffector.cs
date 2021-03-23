using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PulsingEffectType
{
    NONE = 0,
    SCALE = 1 << 0,
    ROTATIONX = 1 << 1,
    ROTATIONY = 1 << 2,
    ROTATIONZ = 1 << 3,
}

public class PulsingEffector : MonoBehaviour
{
    Vector3 defaultScale;
    private float defaultRotx, defaultRoty, defaultRotz;
    AnimationCurve curve;
    PulsingEffectType type;
    float pulsingEffectStartTime = 0f;
    bool isFreshInstance = true;

    public float duration;
    //
    internal void StartPulsing(float duration, AnimationCurve pulseCurve, PulsingEffectType effectType)
    {
        this.duration = duration;
        pulsingEffectStartTime = Time.time;
        curve = pulseCurve;
        type = effectType;

        if (isFreshInstance)
        {
            defaultScale = transform.localScale;
            defaultRotx = transform.localRotation.eulerAngles.x;
            defaultRoty = transform.localRotation.eulerAngles.y;
            defaultRotz = transform.localRotation.eulerAngles.z;
        }

        isFreshInstance = false;
    }

    internal void StartPulsing(AnimationCurve pulseCurve, PulsingEffectType effectType)
    {
        duration = float.MaxValue;
        curve = pulseCurve;
        type = effectType;

        if (isFreshInstance)
        {
            defaultScale = transform.localScale;
            defaultRotx = transform.localRotation.eulerAngles.x;
            defaultRoty = transform.localRotation.eulerAngles.y;
            defaultRotz = transform.localRotation.eulerAngles.z;
        }
        isFreshInstance = false;
    }
    private void Update()
    {
        duration -= Time.deltaTime;
        float curveValue = curve.Evaluate(Time.time - pulsingEffectStartTime);

        if (type == PulsingEffectType.SCALE)
            transform.localScale = defaultScale * (1 + curveValue);

        if (type.HasFlag(PulsingEffectType.ROTATIONX) || type.HasFlag(PulsingEffectType.ROTATIONY) || type.HasFlag(PulsingEffectType.ROTATIONZ))
        {
            float RotX = (type.HasFlag(PulsingEffectType.ROTATIONX)) ? defaultRotx + curveValue : transform.localRotation.eulerAngles.x;
            float RotY = (type.HasFlag(PulsingEffectType.ROTATIONY)) ? defaultRoty + curveValue : transform.localRotation.eulerAngles.y;
            float RotZ = (type.HasFlag(PulsingEffectType.ROTATIONZ)) ? defaultRotz + curveValue : transform.localRotation.eulerAngles.z;
            transform.localRotation = Quaternion.Euler(RotX, RotY, RotZ);
        }

        if (duration <= 0)
            Stop();
    }

    public void Stop()
    {
        if (type == PulsingEffectType.SCALE)
            transform.localScale = defaultScale;

        if (type.HasFlag(PulsingEffectType.ROTATIONX) || type.HasFlag(PulsingEffectType.ROTATIONY) || type.HasFlag(PulsingEffectType.ROTATIONZ))
        {
            float RotX = (type.HasFlag(PulsingEffectType.ROTATIONX)) ? defaultRotx : transform.localRotation.eulerAngles.x;
            float RotY = (type.HasFlag(PulsingEffectType.ROTATIONY)) ? defaultRoty : transform.localRotation.eulerAngles.y;
            float RotZ = (type.HasFlag(PulsingEffectType.ROTATIONZ)) ? defaultRotz : transform.localRotation.eulerAngles.z;
            transform.localRotation = Quaternion.Euler(RotX, RotY, RotZ);
        }

        Destroy(this);
    }
}
