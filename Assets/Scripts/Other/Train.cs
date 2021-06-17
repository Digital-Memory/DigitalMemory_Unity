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
    [SerializeField] float horsePower = 0.05f;
    [ShowNonSerializedField] bool switchIsUp = false;
    [ShowNonSerializedField] bool hasBornPassengers = false;
    [SerializeField] [Range(0, 1)] float position = 0f;
    [ShowNonSerializedField] float acceleration = 0f;

    [SerializeField] Effect spawnOswiecimCitizenEffect;

    bool hasGreen = false;

    public override bool Try(bool on)
    {
        hasGreen = true;
        return true;
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
            targetSpeed = horsePower;

            if (!hasBornPassengers && switchIsUp && position > 0.6f)
            {
                Debug.Log($"{name} spawns passengers");

                for (int i = 0; i < 3; i++)
                {
                    Game.EffectHandler.Play(spawnOswiecimCitizenEffect, gameObject);
                }
                hasBornPassengers = true;
            }

            if (position > 1)
                Reset();
        }


        position += Accelerate(targetSpeed) * Time.deltaTime;
        animator.SetFloat(trainPosition, position);
    }

    private void Reset()
    {
        position = 0;
        hasGreen = false;
        hasBornPassengers = false;
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
        return position < 0.1f || position > 0.4f || hasGreen;
    }
}
