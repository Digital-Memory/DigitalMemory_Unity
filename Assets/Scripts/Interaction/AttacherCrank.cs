using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class AttacherCrank : Attacher
{
    [SerializeField] float startValue, minValue, maxValue;
    [SerializeField] float factor = 360f;
    float currentValue;

    void Awake()
    {
        attachmentName = "Crank";
        Start();
    }

    internal bool TryTurn(float degrees)
    {

        float newValue = currentValue + (degrees / factor);

        if (newValue < maxValue && newValue > minValue)
        {
            currentValue = newValue;
            return true;
        }
        else
        {
            if (newValue < minValue && Mathf.Abs(newValue - minValue) > (180f / factor)
                && newValue > maxValue && Mathf.Abs(newValue - maxValue) > (180f / factor))
                Game.MouseInteractor.ForceEndDrag();

            return false;
        }
    }

    void OnDrawGizmos()
    {
        Vector3 center = transform.position + Vector3.up * 2 + Vector3.forward;

        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(center, Vector3.one * 1.5f);

        float progress = ((currentValue - minValue) / (maxValue - minValue));

        Gizmos.color = Color.Lerp(Color.red, Color.green, progress);
        Gizmos.DrawWireCube(center, Vector3.one * (0.5f + progress));
    }
}
