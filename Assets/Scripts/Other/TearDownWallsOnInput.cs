using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TearDownWallsOnInput : MovingObject
{
    [SerializeField] int currentStage = 0;
    [SerializeField] int amountOfStages = 2;
    [SerializeField] List<Vector3> stages = new List<Vector3>();

    protected override void OnEnable()
    {

        stages.Add(localPositionFalse);

        if (amountOfStages > 1)
        {
            for (int i = 1; i < amountOfStages; i++)
            {
                float lerp = ((float)i / (float)amountOfStages);
                stages.Add(Vector3.Lerp(localPositionFalse, localPositionTrue, lerp));
            }
        }

        stages.Add(localPositionTrue);

        base.OnEnable();

        SetAnimating(false);
    }

    public override bool Try()
    {
        Debug.Log("Receive inpulse (" + name + ")");
        return Try(true);
    }

    public override bool Try(bool b)
    {
        Debug.Log("Try play stage: " + currentStage);

        if (currentStage + 1 >= stages.Count)
            return false;

        UpdatePostionsBasedOnStage();

        if (base.Try(b))
        {
            Debug.Log("playing stage successfull, next stage: " + (currentStage += 1));
            time = b ? 0.01f : 0.99f;
            currentStage++;
            return true;
        }

        return false;
    }

    private void UpdatePostionsBasedOnStage()
    {
        Debug.LogWarning($"current stage {currentStage}");
        localPositionFalse = stages[currentStage];
        localPositionTrue = stages[currentStage + 1];
    }
}
