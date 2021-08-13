using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTempUIHandler : Singleton<CustomTempUIHandler>
{
    [SerializeField] BoxCollider bounds;

    List<Vector3> points;
    List<HighlightTime> highlights;

    public void DrawAnimationCurve(AnimationCurve curve, params float[] timesToHighlight)
    {
        int timeSegments = 32;

        float curveStartTime = curve.keys[0].time;
        float curveEndTime = curve.keys[curve.length - 1].time;

        points = new List<Vector3>();

        for (int i = 0; i <= timeSegments; i++)
        {
            float t = curveStartTime + (curveEndTime - curveStartTime) * ((float)i / (float)timeSegments);
            points.Add(Remap(new Vector2(t, curve.Evaluate(t)), curve));
        }

        highlights = new List<HighlightTime>();

        foreach (float time in timesToHighlight)
        {
            highlights.Add(new HighlightTime(Remap(new Vector2(time, -1000f), curve), Remap(new Vector2(time, 1000f), curve)));
        }
    }

    private Vector3 Remap(Vector2 value, AnimationCurve inCurve)
    {
        Vector2 fromMin = new Vector2(inCurve.keys[0].time, inCurve.keys[0].value);
        Vector2 fromMax = new Vector2(inCurve.keys[inCurve.length - 1].time, inCurve.keys[inCurve.length - 1].value);
        return transform.TransformPoint(Remap(value, fromMin, -(bounds.size / 2), fromMax, bounds.size / 2));
    }

    private Vector3 Remap(Vector2 value, Vector2 fromMin, Vector2 fromMax, Vector2 toMin, Vector2 toMax)
    {
        float x = (value.x - fromMin.x) / (toMin.x - fromMin.x) * (toMax.x - fromMax.x) + fromMax.x;
        float y = (value.y - fromMin.y) / (toMin.y - fromMin.y) * (toMax.y - fromMax.y) + fromMax.y;

        return new Vector3(x, y, 0);
    }

    //private float Remap(this float value, float from1, float to1, float from2, float to2)
    //{
    //    return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    //}

    private void OnDrawGizmos()
    {
        if (points != null)
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (i > 0)
                {
                    Gizmos.DrawLine(points[i-1], points[i]);
                }
            }
        }

        float hue = 0;

        if (highlights != null)
        {
            foreach (var line in highlights)
            {
                Gizmos.color = Color.HSVToRGB(hue, 1, 1);
                hue += 0.1f;
                Gizmos.DrawLine(line.point1, line.point2);
            }
        }

        Gizmos.DrawLine(transform.TransformPoint(-6.5f, -3.5f, 0), transform.TransformPoint(6.5f, -3.5f, 0));
    }
}

public class HighlightTime
{
    public Vector3 point1, point2;
    public HighlightTime(Vector3 p1, Vector3 p2)
    {
        point1 = p1;
        point2 = p2;
    }
}
