using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class AnimatingObject : ConditionedObject
{
    [ShowNonSerializedField]
    Animator animator;
    [SerializeField]
    [AnimatorParam("animator")]
    string varibleFloat;

    [CurveRange(0f, 0f, 1f, 1f)]
    [SerializeField] AnimationCurve animationCurve;
    [SerializeField] float durationInSeconds = 1f;

    float time = 0f;
    bool isAnimating = false;

    private void Reset()
    {
        animator = GetComponent<Animator>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        animator = GetComponent<Animator>();
    }

    public override bool Try()
    {
        if (base.Try())
        {
            isAnimating = true;
            return true;
        }

        return true;
    }

    public override bool Try(bool b)
    {
        if (b)
        {
            Try();
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool Try(float progress)
    {
        if (base.Try(progress))
        {
            UpdateAnimationVariable(progress);
            return true;
        }

        return false;
    }

    private void Update()
    {
        if (isAnimating)
        {
            UpdateAnimationVariable(animationCurve.Evaluate(time));
            time += (Time.deltaTime / durationInSeconds);
            if (time >= 1f)
            {
                isAnimating = false;
            }
        }
    }

    private void UpdateAnimationVariable(float progress)
    {
        if (animator != null && varibleFloat != "")
            animator.SetFloat(varibleFloat, progress);
    }
}
