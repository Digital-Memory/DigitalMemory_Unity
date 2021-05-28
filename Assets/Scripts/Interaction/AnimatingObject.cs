using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(Animator))]
public class AnimatingObject : ChangingOverTimeObject
{
    [ShowNonSerializedField]
    Animator animator;
    [SerializeField]
    [AnimatorParam("animator")]
    string varibleFloat;

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
            animator.SetFloat(varibleFloat, progress);
        }
    }
}
