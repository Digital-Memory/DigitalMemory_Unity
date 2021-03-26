using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTimeChangeSetActive : MonoBehaviour
{
    public TimePoint time;

    private void Start()
    {
        Game.TimeHandler.OnSetTime += OnTimeChange;
    }

    private void OnDestroy()
    {
        Game.TimeHandler.OnSetTime -= OnTimeChange;
    }

    private void OnTimeChange (TimePoint time)
    {
        Debug.LogWarning(time == this.time);

        gameObject.SetActive(time == this.time);
    }
}
