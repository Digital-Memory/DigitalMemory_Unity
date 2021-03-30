using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class AnimatingObject : ConditionedObject
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    [AnimatorParam("animator")]
    string varibleFloat;

    public override bool Try(float progress)
    {
        UpdateAnimationVariable(progress);
        return base.Try(progress);
    }

    private void UpdateAnimationVariable(float progress)
    {
        if (animator != null && varibleFloat != "")
            animator.SetFloat(varibleFloat,progress);
    }
}
