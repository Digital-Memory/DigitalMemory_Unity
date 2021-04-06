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

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override bool Try(float progress)
    {
        Debug.Log("ahahahah");

        UpdateAnimationVariable(progress);
        return base.Try(progress);
    }

    private void UpdateAnimationVariable(float progress)
    {
        if (animator != null && varibleFloat != "")
            animator.SetFloat(varibleFloat,progress);
    }
}
