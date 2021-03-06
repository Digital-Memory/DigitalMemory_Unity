using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(Animator))]
public class AnimatingObject : ChangingOverTimeObject
{
    [ShowNonSerializedField]
    protected Animator animator;
    [SerializeField]
    [AnimatorParam("animator")]
    protected string varibleFloat;

#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();
        animator = GetComponent<Animator>();
    }

#endif

    protected override void OnEnable()
    {
        animator = GetComponent<Animator>();
        base.OnEnable();
    }

    protected override void UpdateChange(float progress)
    {
        if (animator != null && varibleFloat != "")
        {
            if (progress >= 1)
                progress = 0.999f;

            animator.SetFloat(varibleFloat, progress);
        }
    }
}
