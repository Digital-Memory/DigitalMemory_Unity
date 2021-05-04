using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatch : ConditionedObject
{
    [SerializeField] Transform doorRight, doorLeft;
    [SerializeField] AnimationCurve doorZRotation;
    [Expandable]
    [SerializeField] Effect openningEffect;
    [SerializeField] Vector3 axisMultiplier;

    bool isOpen;
    bool isAnimating;
    float clickedTimestamp;
    float rotationAnimationDuration;

    public override bool Try()
    {
        if (base.Try())
            TryOpen();
        else
            TryClose();

        return true;
    }

    public override bool Try(bool on)
    {
        if (base.Try(on))
        {
            if (on)
                TryOpen();
            else
                TryClose();

            return true;
        }

        return false;
    }


    [Button]
    public void TryOpen()
    {
        if (isOpen)
            return;

        isAnimating = true;
        clickedTimestamp = Time.time;
        rotationAnimationDuration = doorZRotation[doorZRotation.length - 1].time;
        isOpen = true;

        Game.EffectHandler.Play(openningEffect, gameObject);
    }

    [Button]
    public void TryClose()
    {
        if (!isOpen)
            return;

        isAnimating = true;
        clickedTimestamp = Time.time;
        rotationAnimationDuration = doorZRotation[doorZRotation.length - 1].time;
        isOpen = false;
    }

    public bool IsClickable()
    {
        return !isAnimating;
    }

    private void Update()
    {
        if (isAnimating)
        {
            if (Time.time > clickedTimestamp + rotationAnimationDuration)
            {
                isAnimating = false;
            }
            else
            {
                float rotation = doorZRotation.Evaluate(isOpen ? (Time.time - clickedTimestamp) : ((Time.time - clickedTimestamp) * -1));
                if (doorLeft != null)
                    doorLeft.localRotation = Quaternion.Euler(axisMultiplier * rotation);
                if (doorRight != null)
                    doorRight.localRotation = Quaternion.Euler(axisMultiplier * -rotation);
            }
        }
    }
}
