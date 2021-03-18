using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeListener : MonoBehaviour
{
    protected Animator animator;
    private void OnEnable()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogWarning("No Animator Referenced at " + gameObject + ". Please add one or remove the TimeListener script.");
        }

        animator.keepAnimatorControllerStateOnDisable = true;
        Game.TimeHandler.OnTimeUpdate += OnTimeUpdate;
    }

    private void OnDisable()
    {
        Game.TimeHandler.OnTimeUpdate -= OnTimeUpdate;
    }

    protected virtual void OnTimeUpdate(float newTime)
    {
        animator.SetFloat("time", newTime);
    }
}
