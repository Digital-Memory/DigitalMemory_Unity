using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : ConditionedObject
{
    [SerializeField] Animator animator;
    [AnimatorParam("animator")]
    [SerializeField] string triggerPass, triggerEnter, trainPosition;
    [ShowNonSerializedField] bool forwardHasGreen;
    [ShowNonSerializedField] bool facingForward = true;
    [ShowNonSerializedField] bool switchIsUp = false;
    [SerializeField] [Range(0, 1)] float position = 0f;
    [ShowNonSerializedField] float acceleration = 0f;

    [SerializeField] Effect spawnOswiecimCitizenEffect;

    public override bool Try(float progress)
    {
        if (forwardHasGreen)
        {
            if (progress < 0.1f)
            {
                SetGreenForward(false);
            }
        }
        else
        {
            if (progress > 0.9f)
            {
                SetGreenForward(true);
            }
        }

        return true;
    }

    private void SetGreenForward(bool forward)
    {
        forwardHasGreen = forward;
    }

    private void Update()
    {
        if (position < 0.3f)
        {
            if (switchIsUp != SwitchIsUp())
            {
                switchIsUp = !switchIsUp;
                animator.SetTrigger(switchIsUp ? triggerEnter : triggerPass);
            }
        }

        float targetSpeed = 0;

        if (CanDrive())
        {
            targetSpeed = (facingForward ? 0.1f : -0.1f);

            if (position > 1)
                TurnBack();
            else if (position < 0)
                TurnForward();
        }


        position += Accelerate(targetSpeed) * Time.deltaTime;
        animator.SetFloat(trainPosition, position);
    }

    private void TurnForward()
    {
        if (!facingForward)
        {
            facingForward = true;
        }
    }

    private void TurnBack()
    {
        if (facingForward)
        {
            if (switchIsUp)
            {
                for (int i = 0; i < 3; i++)
                {
                    Game.EffectHandler.Play(spawnOswiecimCitizenEffect,gameObject);
                }
            }
            facingForward = false;
        }
    }

    private float Accelerate(float target)
    {
        acceleration = Mathf.MoveTowards(acceleration, target, Time.deltaTime * 0.1f);
        return acceleration;
    }

    private bool SwitchIsUp()
    {
        return CheckAllConditionsForTrue();
    }

    private bool CanDrive()
    {
        return position < 0.1f || position > 0.4f || (facingForward == forwardHasGreen);
    }
}
