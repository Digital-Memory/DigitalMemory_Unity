using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FloatSender))]
public class FloatSenderMoveBack : ConditionedObject
{
    FloatSender floatSender;
    [SerializeField] float baseValue = 0, targetValue = 1, maxDistanceFromTargetToNotMoveBack = 0.1f;

    [SerializeField] bool moveOnInput = false;
    [SerializeField] [ShowIf("moveOnInput")] bool toBase = true;

    protected override void OnEnable()
    {
        base.OnEnable();

        floatSender = GetComponent<FloatSender>();
        if (floatSender != null)
        {
            floatSender.OnStartPlayerInput += StopMoveBack;
            floatSender.OnEndPlayerInput += TryMoveBack;
        }
    }

    private void OnDisable()
    {
        if (floatSender != null)
        {
            floatSender.OnStartPlayerInput -= StopMoveBack;
            floatSender.OnEndPlayerInput -= TryMoveBack;
        }
    }

    private void StopMoveBack()
    {
        StopAllCoroutines();
    }

    public override bool Try()
    {

        float currentValue = floatSender.CurrentValue;
        StartCoroutine(MoveBackRoutine(currentValue, toBase ? baseValue : targetValue));

        return base.Try();
    }

    private void TryMoveBack()
    {
        StopAllCoroutines();

        float currentValue = floatSender.CurrentValue;
        bool upwardsDirection = baseValue < targetValue;

        if (upwardsDirection ? currentValue > baseValue : currentValue < baseValue)
        {

            bool targetReached = upwardsDirection ?
                currentValue < (targetValue - maxDistanceFromTargetToNotMoveBack) :
                currentValue > (targetValue + maxDistanceFromTargetToNotMoveBack);

                StartCoroutine(MoveBackRoutine(currentValue, targetReached ? baseValue : targetValue));
        }
    }

    private IEnumerator MoveBackRoutine(float currentValue, float targetValue)
    {
        float lastdistance = float.MaxValue;
        float newDistance = 100f;

        while (newDistance < lastdistance)
        {
            yield return null;
            lastdistance = newDistance;
            newDistance = Mathf.Abs(currentValue - targetValue);

            currentValue = Mathf.MoveTowards(currentValue, targetValue, Time.deltaTime);
            floatSender.TryGiveInputRaw(currentValue);
        }

        currentValue = targetValue;
        floatSender.TryGiveInputRaw(currentValue);
    }
}
