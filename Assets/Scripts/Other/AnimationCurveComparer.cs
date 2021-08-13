using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCurveComparer : MonoBehaviour
{
    [SerializeField] AnimationCurve curve1, curve2;
    AnimationCurve curve2Inverted;


    // Update is called once per frame
    void Update()
    {
        float startTime = curve2[0].time;
        float endTime = curve2[curve2.length - 1].time;

        List<Keyframe> keyframes = new List<Keyframe>();

        for (int i = 0; i < 128; i++)
        {
            float value = startTime + (((float)i / 128f) * (endTime - startTime));
            float time = curve2.Evaluate(value);
            keyframes.Add(new Keyframe(time, value));
        }

        curve2Inverted = new AnimationCurve(keyframes.ToArray());

        Gizmos.color = Color.cyan;
        DebugDraw.AnimationCurves(curve1, curve2Inverted);
    }
}
