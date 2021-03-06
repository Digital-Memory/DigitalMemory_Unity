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

    [ShowNonSerializedField] int passengerGroupsBorn = 0;
    
    [SerializeField] [Range(0, 1)] float position = 0f;
    [ShowNonSerializedField] float acceleration = 0f;

    [SerializeField] Transform trainStation;
    [SerializeField] InventoryObjectData oswiecimCitizen;
    [SerializeField] AudioSource trainAudio;

    bool hasGreen = false;
    private int passengersBorn = 0;
    private float passengerBirthtime;

    public override bool Try()
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
            if (passengerGroupsBorn < 3 && passengersBorn < 3 && switchIsUp && position > 0.6f)
            {
                passengerBirthtime += Time.deltaTime * 2f;

                if (Mathf.FloorToInt(passengerBirthtime) == (passengersBorn + 1))
                {
                    passengersBorn++;
                    Vector2 pos = Game.CameraController.Camera.WorldToScreenPoint(trainStation.position);
                    Game.UIHandler.InventoryAdder.MoveToInventory(oswiecimCitizen, pos);
                }
                
                if(passengersBorn >= 3)
                    passengerGroupsBorn += 1;
            } else
            {
                targetSpeed = horsePower;
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
        passengersBorn = 0;
        passengerBirthtime = 0;
    }

    private float Accelerate(float target)
    {
        acceleration = Mathf.MoveTowards(acceleration, target, Time.deltaTime * 0.1f);
        trainAudio.pitch = (acceleration / horsePower);
        return acceleration;
    }

    private bool SwitchIsUp()
    {
        return CheckAllConditionsForTrue();
    }

    private bool CanDrive()
    {
        return position < 0.15f || position > 0.4f || hasGreen;
    }

    internal float GetTrainPosition()
    {
        return position;
    }
}
