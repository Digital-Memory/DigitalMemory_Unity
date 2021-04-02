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
    float snapValue;
    bool isSnapping = false;

    FloatSender sender;

    private void OnEnable()
    {
        sender = GetComponent<FloatSender>();
        snapValues = snapValues.OrderBy(f => f).ToList();
    }

    private void OnValueChangedCallback()
    {
        snapValues = snapValues.Where(f => f <= 1f && f >= 0).ToList();
    }

    internal void StopAnySnap()
    {
        isSnapping = false;
    }

    internal void TrySnap()
    {
        float currentValue = sender.CurrentValue;
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
            float current = sender.CurrentValue;
            float distance = Mathf.Abs(current - snapValue);

            if (distance < reachedDistance)
            {
                Debug.Log("End Snap");
                isSnapping = false;
            }
            else
            {
                sender.TryGiveInput(sender.Factorize(Mathf.MoveTowards(current, snapValue, Time.deltaTime)), isAbsolute:true);
            }
        }
    }
}
