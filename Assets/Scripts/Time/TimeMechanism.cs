using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMechanism : MovingObject
{
    [SerializeField] FloatSender crankToReset;
    [SerializeField] AnimationCurve resetCurve;
    [SerializeField] FloatSnapper leverSnapper;

    float leverPosition;
    Dictionary<TimePoint, float> timePoints = new Dictionary<TimePoint, float>();
    [SerializeField] List<TimePoint> tptp;
    [SerializeField] List<float> tpfl;

    AttacherTimePlate[] attachersTimePlate;

    protected override void OnEnable()
    {
        attachersTimePlate = FindObjectsOfType<AttacherTimePlate>();
        foreach (AttacherTimePlate attacher in attachersTimePlate)
        {
            attacher.OnAttachPlateOnPosition += OnAttachPlateOnPosition;
            attacher.OnDetachPlateOnPosition += OnDetachPlateOnPosition;
        }
        base.OnEnable();
    }


    private void OnDisable()
    {
        foreach (AttacherTimePlate attacher in attachersTimePlate)
        {
            attacher.OnAttachPlateOnPosition -= OnAttachPlateOnPosition;
            attacher.OnDetachPlateOnPosition -= OnDetachPlateOnPosition;
        }
    }

    void OnAttachPlateOnPosition(float associatedLeverPosition, TimePoint pointToSet)
    {
        if (timePoints.ContainsKey(pointToSet))
            timePoints[pointToSet] = associatedLeverPosition;
        else
            timePoints.Add(pointToSet, associatedLeverPosition);

        UpdateSnapValues();
    }

    private void UpdateSnapValues()
    {
        leverSnapper.DefineSnapValues(new List<float>(timePoints.Values).ToArray());
    }

    void OnDetachPlateOnPosition(float position)
    {
        bool found = false;
        TimePoint toRemove = TimePoint.POST_WAR;

        foreach (KeyValuePair<TimePoint, float> item in timePoints)
        {
            if (Mathf.Abs(item.Value - position) < 0.01f)
            {
                found = true;
                toRemove = item.Key;
            }
        }

        if (found)
            timePoints.Remove(toRemove);
    }


    public override bool Try(bool b)
    {
        TimePoint selected = GetClosestSelectedTimePoint(leverPosition);
        if (selected != Game.TimeHandler.CurrentTime)
        {
            Game.TimeHandler.SetTime(selected);
            StopAllCoroutines();
            StartCoroutine(ResetCrankRoutine());
            return true;
        } else {
            return false;
        }
    }

    private TimePoint GetClosestSelectedTimePoint(float leverPosition)
    {
        TimePoint tp = Game.TimeHandler.CurrentTime;
        float distance = float.MaxValue;

        foreach (KeyValuePair<TimePoint, float> item in timePoints)
        {
            float dist = Mathf.Abs(leverPosition - item.Value);
            if (dist < distance) {
                distance = dist;
                tp = item.Key;
            }
        }

        return tp;
    }

    private IEnumerator ResetCrankRoutine()
    {
        float t = 0;
        float timeMax = resetCurve.keys[resetCurve.length - 1].time;

        while (time < timeMax) {
            t += Time.deltaTime;
            crankToReset.TryGiveInputRaw(resetCurve.Evaluate(t));
            yield return null;
        }
    }

    public override bool Try(float progress)
    {
        if (base.Try(progress)) {
            leverPosition = progress;
            return true;
        }
        return false;
    }

    private void Update()
    {
        tptp = new List<TimePoint>();
        tpfl = new List<float>();
        foreach (KeyValuePair<TimePoint, float> item in timePoints)
        {
            tptp.Add(item.Key);
            tpfl.Add(item.Value);
        }
    }
}
