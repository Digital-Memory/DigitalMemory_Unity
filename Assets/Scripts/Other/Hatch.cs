using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatch : ConditionedObject
{
    [SerializeField] protected Transform doorRight, doorLeft;
    [SerializeField] protected AnimationCurve doorZRotation;
    [Expandable]
    [SerializeField] protected Effect openningEffect;
    [SerializeField] protected Vector3 axisMultiplier;
    [SerializeField] protected Vector3 baseRotation;

    bool isOpen;
    bool isAnimating;
    float time;

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

        isOpen = true;
        StartAnimating();

        Game.EffectHandler.Play(openningEffect, gameObject);
    }

    [Button]
    public void TryClose()
    {
        if (!isOpen)
            return;

        isOpen = false;
        StartAnimating();
    }

    public bool IsClickable()
    {
        return !isAnimating;
    }

    private void Update()
    {
        if (isAnimating)
        {
            time += (isOpen ? 1f : -1f) * Time.deltaTime;

            if (isOpen ? (time < 1f) : (time > 0f))
            {
                SetRotationFromTime(time);
            }
            else
            {
                time = isOpen ? 1 : 0;
                SetRotationFromTime(time);
                EndAnimating();
            }
        }
    }

    private void SetRotationFromTime(float time)
    {
        float rotation = doorZRotation.Evaluate(time);
        if (doorLeft != null)
            doorLeft.localRotation = Quaternion.Euler(axisMultiplier * rotation + baseRotation);
        if (doorRight != null)
            doorRight.localRotation = Quaternion.Euler(axisMultiplier * -rotation + baseRotation);
    }

    protected virtual void StartAnimating()
    {
        isAnimating = true;
    }

    protected virtual void EndAnimating()
    {
        isAnimating = false;
    }
}
