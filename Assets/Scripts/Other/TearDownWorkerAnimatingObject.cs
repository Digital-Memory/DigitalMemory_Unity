using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TearDownWorkerAnimatingObject : AnimatingObject
{
    AnimationCurve curve;
    [SerializeField] AnimationCurve chargeCurve, hitCurve;
    bool isReceivingFloatInput = true;

    public override bool Try(bool b)
    {
        if (CheckAllConditionsForTrue())
        {
            time = 0;
            curve = hitCurve;
            isReceivingFloatInput = false;
            return base.Try(b);
        }

        return false;
    }

    public override bool Try(float progress)
    {
        if (!isReceivingFloatInput)
            return true;

        if (!CheckAllConditionsForTrue())
        {
            curve = chargeCurve;
            time = 0;
            UpdateChange(progress);
            return true;
        }

        return true;
    }

    protected override void OnFinishedAnimating()
    {
        isReceivingFloatInput = true;
        base.OnFinishedAnimating();
    }

    protected override void UpdateChange(float progress)
    {
        if (animator != null && varibleFloat != "")
        {
            if (progress >= 1)
                progress = 0.999f;

            //Debug.Log($"{name} => set animator float ({varibleFloat}) : {progress}");
            animator.SetFloat(varibleFloat, curve.Evaluate(progress));
        }
    }
}
