using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class TearDownWallsOnInput : MovingObject
{
    int currentStage = 0;
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

    public override bool Try(bool b)
    {
        UpdatePostionsBasedOnStage();

        if (base.Try(b))
        {
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
