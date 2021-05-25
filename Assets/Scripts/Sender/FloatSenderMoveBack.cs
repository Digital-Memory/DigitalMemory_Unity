using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FloatSender))]
public class FloatSenderMoveBack : MonoBehaviour
{
    FloatSender floatSender;
    [SerializeField] float baseValue = 0, targetValue = 1, maxDistanceFromTargetToNotMoveBack = 0.1f;

    private void OnEnable()
    {
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
        float newDistance = 1f;

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
