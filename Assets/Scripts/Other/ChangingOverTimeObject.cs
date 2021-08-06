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
        animationCurve = AnimationCurve.EaseInOut(0,0,1,1);
    }

#endif

    protected float time = 1f;
    protected float direction = 1;
    protected bool isAnimating = false;

    protected override void OnEnable()
    {
        base.OnEnable();

        time = animateOnStart != startFromEnd ? 0.99f : 0.01f;
        Try(startFromEnd);
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
        //if (animating)
        //    Debug.Log($"{name} Starts Animating: {direction} t: {time}");
        //else
        //    Debug.Log($"{name} Finishes Animating: {direction} t: {time}");
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
            UpdateChange(animationCurve.Evaluate(time));
            time += (Time.deltaTime / durationInSeconds) * direction;
            if (time > 1f || time < 0f)
            {
                time = time > 0.5f != resetAtEnd ? 1f : 0f;

                SetAnimating(false);
                OnFinishedAnimating();
            }
        }
    }

    protected virtual void OnFinishedAnimating()
    {

    }

    protected virtual void UpdateChange(float progress)
    {
        
    }
}
