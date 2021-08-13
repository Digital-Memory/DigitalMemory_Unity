using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionListenerTrain : ConditionListenerBehaviour
{
    Train train;

    private void OnEnable()
    {
        train = GetComponent<Train>();
    }

    public override bool SupportsBool()
    {
        return true;
    }

    public override bool GetBool()
    {
        float pos = train.GetTrainPosition();
        return pos < 0.3f || pos > 0.6f;
    }
}
