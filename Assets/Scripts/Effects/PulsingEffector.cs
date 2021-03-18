using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsingEffector : MonoBehaviour
{
    Vector3 defaultScale;
    AnimationCurve curve;

    private void Start()
    {
        defaultScale = transform.localScale;
    }

    public float duration;
    //
    internal void StartPulsing(float duration, AnimationCurve pulseCurve)
    {
        this.duration = duration;
        curve = pulseCurve;
    }

    internal void StartPulsing(AnimationCurve pulseCurve)
    {
        duration = float.MaxValue;
        curve = pulseCurve;
    }
    private void Update()
    {
        duration -= Time.deltaTime;
        transform.localScale = defaultScale * curve.Evaluate(Time.time);

        if (duration <= 0)
            Stop();
    }

    public void Stop()
    {
        transform.localScale = defaultScale;
        Destroy(this);
    }
}
