using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimePoint
{
    PRE_WAR,
    WAR,
    POST_WAR,
    PRESENT,
}

public class TimeHandler : Singleton<TimeHandler>
{
    public TimePoint CurrentTime { get; internal set; }

    public event System.Action<TimePoint> OnSetTime;

    protected override void Start()
    {
        base.Start();
        SetTime(TimePoint.PRE_WAR);
    }

    public void SetTime(TimePoint toSet)
    {
        Debug.LogWarning("Set " + toSet.ToString());
        CurrentTime = toSet;
        OnSetTime?.Invoke(toSet);
    }
}
