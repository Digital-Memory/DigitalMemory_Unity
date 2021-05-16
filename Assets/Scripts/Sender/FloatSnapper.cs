using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Linq;

[RequireComponent(typeof(FloatSender))]
public class FloatSnapper : MonoBehaviour
{
    [ReorderableList]
    [OnValueChanged("OnValueChangedCallback")]
    public List<float> snapValues = new List<float>(new float[] { 0f, 1f });
    public float snapDistance = 0.1f;
    public float reachedDistance = 0.01f;

    [Foldout("Effects")] [SerializeField] [Expandable] Effect startSnapEffect, endSnapEffect;

    float snapValue;
    bool isSnapping = false;

    FloatSender floatSender;


    private void OnEnable()
    {
        floatSender = GetComponent<FloatSender>();
        if (floatSender != null)
        {
            floatSender.OnStartPlayerInput += StopAnySnap;
            floatSender.OnEndPlayerInput += TrySnap;
        }

        snapValues = snapValues.OrderBy(f => f).ToList();
    }

    private void OnDisable()
    {
        if (floatSender != null)
        {
            floatSender.OnStartPlayerInput -= StopAnySnap;
            floatSender.OnEndPlayerInput -= TrySnap;
        }
    }

    private void OnValueChangedCallback()
    {
        snapValues = snapValues.Where(f => f <= 1f && f >= 0).ToList();
    }

    internal void StopAnySnap()
    {
        Debug.Log("End Snap");
        isSnapping = false;
    }

    internal void TrySnap()
    {
        float currentValue = floatSender.CurrentValue;
        snapValue = GetClosestSnapValue(currentValue);

        if (Mathf.Abs(currentValue - snapValue) < snapDistance)
        {
            Debug.Log("Start Snap");
            isSnapping = true;
        }
    }

    private float GetClosestSnapValue(float currentValue)
    {
        return snapValues.OrderBy(snapValue => Mathf.Abs(snapValue - currentValue)).FirstOrDefault();
    }

    private void Update()
    {
        if (isSnapping)
        {
            float current = floatSender.CurrentValue;
            float distance = Mathf.Abs(current - snapValue);

            if (distance < reachedDistance)
            {
                StopAnySnap();
            }
            else
            {
                floatSender.TryGiveInput(floatSender.Factorize(Mathf.MoveTowards(current, snapValue, Time.deltaTime)), isAbsolute:true);
            }
        }
    }
}
