using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryProductionAnimatingObject : AnimatingObject
{
    [HideInInspector] List<ConditionBase> conditions;
    [SerializeField] private List<StageInformation> stageInformation = new List<StageInformation>();
    [SerializeField] ProdctionStage currentStage;
    [SerializeField] FloatSender crank, lever;

    bool moving = false;
    float currentTime, targetTime;
    private ProdctionStage targetStage;
    [SerializeField] float animationSpeed;
    internal void PreviewStage(float time)
    {
        UpdateChange(time);
    }


    //Need to rework this at some point
    public override bool Try(float progress)
    {
        if (moving) return true;

        if ((currentStage == ProdctionStage.Founder || currentStage == ProdctionStage.Welder) && progress > 0.95f)
            MoveToNextStage();

        return true;
    }

    //Need to rework this at some point
    public override bool Try(bool b)
    {
        if (moving) return false;

        if (currentStage == ProdctionStage.Stomper)
            MoveToNextStage();

        return true;
    }

    private void MoveToNextStage()
    {
        ProdctionStage nextStage = (ProdctionStage)(((int)currentStage) + 1);
        ProdctionStage stage = currentStage == ProdctionStage.Finished ? ProdctionStage.Finished : nextStage;
        Debug.Log($"current: {currentStage} => next: {stage}");
        float nextTime = GetTimeOfStage(stage);
        MoveToStage(stage, nextTime);
    }

    private float GetTimeOfStage(ProdctionStage stage)
    {
        foreach (StageInformation info in stageInformation)
        {
            if (info.Stage == stage)
                return info.Time;
        }

        return 0f;
    }

    private void MoveToStage(ProdctionStage nextStage, float nextTime)
    {
        Debug.LogWarning("Move to stage " + nextStage.ToString());
        crank.OverrideInputReference(nextStage == ProdctionStage.Founder ? this : null);
        lever.OverrideInputReference(nextStage != ProdctionStage.Founder ? this : null);
        targetTime = nextTime;
        targetStage = nextStage;
        moving = true;
    }

    private void Update()
    {
        if (moving)
        {
            currentTime = Mathf.MoveTowards(currentTime, targetTime, Time.deltaTime * animationSpeed);
            UpdateChange(currentTime);

            if (Mathf.Abs(currentTime - targetTime) < 0.01f)
            {
                Debug.LogError($"reached: {currentTime} => {targetTime}");
                moving = false;
                currentStage = targetStage;
            }
        }
    }

    private void OnGUI()
    {
        GUILayout.Box(currentStage.ToString() + $" t: {currentTime} => {targetTime}");
    }
}

[System.Serializable]
public class StageInformation
{
    public ProdctionStage Stage;
    [Range(0, 1)]
    public float Time;
}

public enum ProdctionStage
{
    Empty = 0,
    Founder = 1,
    Stomper = 2,
    Welder = 3,
    Finished = 4,
}

