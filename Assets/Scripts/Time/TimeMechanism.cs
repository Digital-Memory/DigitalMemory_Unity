using NaughtyAttributes;
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
    [SerializeField] TimeMechanismNumberDisplayer day1, day2, month1, month2, year1, year2, year3, year4;
    [SerializeField] Timestamp timestamp;

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

    public override bool Try()
    {
        StopAllCoroutines();
        StartCoroutine(TimeTravelRoutine());
        return true;
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

    [Button]
    private void TestTimeTravel()
    {
        StopAllCoroutines();
        StartCoroutine(TimeTravelRoutine());
    }

    private IEnumerator TimeTravelRoutine()
    {
        day1.MoveTo(timestamp.GetDigid(TimestampSpot.Day, 1));
        yield return new WaitForSeconds(0.1f);
        day2.MoveTo(timestamp.GetDigid(TimestampSpot.Day, 2));
        yield return new WaitForSeconds(0.5f);
        month1.MoveTo(timestamp.GetDigid(TimestampSpot.Month, 1));
        yield return new WaitForSeconds(0.1f);
        month2.MoveTo(timestamp.GetDigid(TimestampSpot.Month, 2));
        yield return new WaitForSeconds(0.5f);
        year1.MoveTo(timestamp.GetDigid(TimestampSpot.Year, 1));
        yield return new WaitForSeconds(0.1f);
        year2.MoveTo(timestamp.GetDigid(TimestampSpot.Year, 2));
        yield return new WaitForSeconds(0.1f);
        year3.MoveTo(timestamp.GetDigid(TimestampSpot.Year, 3));
        yield return new WaitForSeconds(0.1f);
        year4.MoveTo(timestamp.GetDigid(TimestampSpot.Year, 4), overshoot: 6);
        yield return new WaitForSeconds(0.1f);
    }
}

[System.Serializable]
public class Timestamp
{
    public int day;
    public int month;
    public int year;

    public int GetDigid(TimestampSpot spot, int index)
    {
        int i = spot == TimestampSpot.Day ? day : spot == TimestampSpot.Month ? month : year;
        int indexFromLast = (spot == TimestampSpot.Year ? 5 : 3) - index;
        float pow = Mathf.Pow(10, indexFromLast);
        return Mathf.FloorToInt((i % pow) / (pow / 10));
    }
}

public enum TimestampSpot
{
    Day,
    Month,
    Year,
}
