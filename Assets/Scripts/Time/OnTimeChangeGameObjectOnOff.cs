using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTimeChangeGameObjectOnOff : MonoBehaviour
{
    [SerializeField] TimePoint timePointActive;
    [SerializeField] GameObject objectToAffect;

    private void OnEnable()
    {
        Game.TimeHandler.OnSetTime += OnTimeChange;
    }

    private void OnDisable()
    {
        Game.TimeHandler.OnSetTime -= OnTimeChange;
    }

    private void OnTimeChange(TimePoint newTimePoint)
    {
        objectToAffect.SetActive(newTimePoint == timePointActive);
    }
}
