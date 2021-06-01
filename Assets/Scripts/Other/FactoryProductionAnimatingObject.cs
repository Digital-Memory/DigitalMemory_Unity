using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class FactoryProductionAnimatingObject : AnimatingObject
{
    [HideInInspector] List<ConditionBase> conditions;
    private List<FactoryProductionStageInformation> stageInformation = new List<FactoryProductionStageInformation>();
    [SerializeField] ProdctionStage currentStage;

    bool moving = false;
    float currentTime, targetTime;
    private ProdctionStage targetStage;
    [SerializeField] float animationSpeed;

#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();
        GetAllStageInformationComponents();
    }

#endif

    protected override void OnEnable()
    {
        base.OnEnable();
        GetAllStageInformationComponents();
    }

    internal void PreviewStage(float time)
    {
        UpdateChange(time);
    }

    private void GetAllStageInformationComponents()
    {
        stageInformation = new List<FactoryProductionStageInformation>(GetComponents<FactoryProductionStageInformation>());
    }

    public override bool Try(float progress)
    {
        if (moving) return false;

        switch (currentStage)
        {
            case ProdctionStage.Founder:
            case ProdctionStage.Welder:
                Debug.Log("Founder Progress: " + progress + $" greate than 95 {progress > 0.95f}.");

                if (progress > 0.95f)
                {
                    MoveToNextStage();
                }
                break;
        }

        return true;
    }

    public override bool Try(bool b)
    {
        if (moving) return false;

        if (currentStage == ProdctionStage.Stomper)
        {
            MoveToNextStage();
        }

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
        foreach (FactoryProductionStageInformation info in stageInformation)
        {
            if (info.Stage == stage)
                return info.Time;
        }

        return 0f;
    }

    private void MoveToStage(ProdctionStage nextStage, float nextTime)
    {
        Debug.LogWarning("Move to stage " + nextStage.ToString());
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

public enum ProdctionStage
{
    Empty = 0,
    Founder = 1,
    Stomper = 2,
    Welder = 3,
    Finished = 4,
}

