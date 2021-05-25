using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class AnimatingObject : ChangingOverTimeObject
{
    [ShowNonSerializedField]
    Animator animator;
    [SerializeField]
    [AnimatorParam("animator")]
    string varibleFloat;

    private void Reset()
    {
        animator = GetComponent<Animator>();
    }

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
