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
    [Foldout("References")] [SerializeField] TimeMechanismNumberDisplayer day1, day2, month1, month2, year1, year2, year3, year4;
    [Foldout("References")] [SerializeField] Attacher[] lightbulbAttachers;
    [Foldout("References")] [SerializeField] MovingObject[] lightbulbAttachersBlends;
    [Foldout("References")] [SerializeField] List<TimeMechanismLightbulb> lightbulbs = new List<TimeMechanismLightbulb>();
    [Foldout("References")] [SerializeField] AudioClip start;
    [SerializeField] Timestamp[] timestamps;

    AttacherTimePlate[] attachersTimePlate;


    protected override void OnEnable()
    {
        foreach (Attacher attacher in lightbulbAttachers)
        {
            attacher.OnChangeAttached += OnAttachLightbulb;
        }

        base.OnEnable();
    }

    protected virtual void Start()
    {
        Timestamp timestamp = timestamps[0];

        day1.MoveTo(timestamp.GetDigid(TimestampSpot.Day, 1), overshoot: 1);
        day2.MoveTo(timestamp.GetDigid(TimestampSpot.Day, 2), overshoot: 1);
        month1.MoveTo(timestamp.GetDigid(TimestampSpot.Month, 1), overshoot: 1);
        month2.MoveTo(timestamp.GetDigid(TimestampSpot.Month, 2), overshoot: 1);
        year1.MoveTo(timestamp.GetDigid(TimestampSpot.Year, 1), overshoot: 1);
        year2.MoveTo(timestamp.GetDigid(TimestampSpot.Year, 2), overshoot: 1);
        year3.MoveTo(timestamp.GetDigid(TimestampSpot.Year, 3), overshoot: 1);
        year4.MoveTo(timestamp.GetDigid(TimestampSpot.Year, 4), overshoot: 1);
    }

    void OnAttachPlateOnPosition(float associatedLeverPosition, TimePoint pointToSet)
    {
        if (timePoints.ContainsKey(pointToSet))
            timePoints[pointToSet] = associatedLeverPosition;
        else
            timePoints.Add(pointToSet, associatedLeverPosition);

        UpdateSnapValues();
    }
    private void OnAttachLightbulb(bool isAttached, string attachment)
    {
        if (isAttached)
        {
            int numberOfLightbulbs = lightbulbs.Count;


            TimeMechanismLightbulb lightbulb = lightbulbAttachers[numberOfLightbulbs].GetComponentInChildren<TimeMechanismLightbulb>();
            if (lightbulb != null && !lightbulbs.Contains(lightbulb))
            {
                lightbulbs.Add(lightbulb);
                timestamps[numberOfLightbulbs].enabled = true;
                UpdateSnapValues();
                UpdateSlotCaps(numberOfLightbulbs + 1);
            }
        }
    }

    private void UpdateSlotCaps(int capIndexToShow)
    {
        if (capIndexToShow >= lightbulbAttachersBlends.Length)
            return;

        //Open the blend for the next Attacher
        MovingObject blend = lightbulbAttachersBlends[capIndexToShow];
        if (blend != null)
            blend.Try(true);
    }

    private void UpdateSnapValues()
    {
        List<float> snaps = new List<float>();

        for (int i = 0; i < timestamps.Length; i++)
            if (timestamps[i].enabled)
                snaps.Add(1f - (float)i / 3f);

        leverSnapper.DefineSnapValues(snaps.ToArray());
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
        }
        else
        {
            return false;
        }
    }

    public override bool Try()
    {
        StopAllCoroutines();
        Timestamp timestamp = timestamps[GetTimestampIndexFromLeverPosition(leverPosition)];

        if (timestamp.enabled)
        {
            StartCoroutine(TimeTravelRoutine(timestamp));
            return true;
        }

        return false;
    }

    private TimePoint GetClosestSelectedTimePoint(float leverPosition)
    {
        TimePoint tp = Game.TimeHandler.CurrentTime;
        float distance = float.MaxValue;

        foreach (KeyValuePair<TimePoint, float> item in timePoints)
        {
            float dist = Mathf.Abs(leverPosition - item.Value);
            if (dist < distance)
            {
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

        while (Time < timeMax)
        {
            t += UnityEngine.Time.deltaTime;
            crankToReset.TryGiveInputRaw(resetCurve.Evaluate(t));
            yield return null;
        }
    }

    public override bool Try(float progress)
    {
        if (base.Try(progress))
        {
            leverPosition = progress;
            UpdateLightbulbs();
            return true;
        }
        return false;
    }

    private void UpdateLightbulbs()
    {
        int indexActive = GetTimestampIndexFromLeverPosition(leverPosition);
        for (int i = 0; i < lightbulbs.Count; i++)
        {
            if (lightbulbs[i] != null)
                lightbulbs[i].SetLightActive(i == indexActive);
        }
    }

    [Button]
    private void TestTimeTravel()
    {
        Try();
    }

    private int GetTimestampIndexFromLeverPosition(float leverPosition)
    {
        if (leverPosition < 0.25f)
            return 3;
        else if (leverPosition < 0.5f)
            return 2;
        else if (leverPosition < 0.75f)
            return 1;
        else
            return 0;
    }

    private IEnumerator TimeTravelRoutine(Timestamp timestamp)
    {
        day1.MoveTo(timestamp.GetDigid(TimestampSpot.Day, 1));
        yield return new WaitForSeconds(0.1f);

        Game.TimeHandler.SetTime(timestamp.point);
        Game.SoundPlayer.Play(start, gameObject, 0.25f, 0.1f);

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

    public bool enabled;

    public TimePoint point;

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
