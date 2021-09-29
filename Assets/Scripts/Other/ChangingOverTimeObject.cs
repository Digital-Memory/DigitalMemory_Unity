using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingOverTimeObject : ConditionedObject
{
    [CurveRange(0f, 0f, 1f, 1f)]
    [SerializeField] protected AnimationCurve animationCurve;
    [SerializeField] float durationInSeconds = 1f;

    [SerializeField] bool startFromEnd = true;
    [SerializeField] bool animateOnStart = true;
    [SerializeField] bool resetAtEnd = false;

#if UNITY_EDITOR

    protected virtual void Reset()
    {
        animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    }

#endif

    [ShowNonSerializedField] private float time = 1f;
    protected float direction = 1;
    protected bool isAnimating = false;

    public System.Action<float> OnTimeChangeEvent;
    public System.Action OnFinishedAnimatingEvent;

    public float Time
    {
        get => time;
        set
        {
            time = value;
            OnTimeChangeEvent?.Invoke(time);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        Time = animateOnStart != startFromEnd ? 0.99f : 0.01f;
        if (animateOnStart)
        {
            direction = Time > 0.5f ? -1f : 1f;
            SetAnimating(true);
        } else
        {
            UpdateChange(Time);
        }
        //Try(startFromEnd);
    }

    public override bool Try()
    {
        if (base.Try())
        {
            direction = 1;
            SetAnimating(true);
            return true;
        }

        return false;
    }

    protected void SetAnimating(bool animating)
    {
        isAnimating = animating;
    }

    public override bool Try(bool b)
    {
        if (base.Try(b))
        {
            if (b)
            {
                direction = 1;
                SetAnimating(true);
                return true;
            }
            else
            {
                direction = -1;
                SetAnimating(true);
                return false;
            }
        }

        return false;
    }

    public override bool Try(float progress)
    {
        if (base.Try(progress))
        {
            UpdateChange(progress);
            return true;
        }

        return false;
    }

    private void Update()
    {
        if (isAnimating)
        {
            UpdateChange(animationCurve.Evaluate(Time));
            Time += (UnityEngine.Time.deltaTime / durationInSeconds) * direction;
            if (Time > 1f || Time < 0f)
            {
                Time = Time > 0.5f != resetAtEnd ? 1f : 0f;

                SetAnimating(false);
                OnFinishedAnimating();
            }
        }
    }

    protected virtual void OnFinishedAnimating()
    {
        OnFinishedAnimatingEvent?.Invoke();
    }

    protected virtual void UpdateChange(float progress)
    {

    }
}
