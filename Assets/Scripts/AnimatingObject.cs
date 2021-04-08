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

    private void Reset()
    {
        animator = GetComponent<Animator>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        animator = GetComponent<Animator>();
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

    private void UpdateAnimationVariable(float progress)
    {
        if (animator != null && varibleFloat != "")
            animator.SetFloat(varibleFloat, progress);
    }
}
