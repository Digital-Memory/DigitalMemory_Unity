using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringObject : ConditionedObject
{
    [ShowNonSerializedField]
    float tension;

    [SerializeField]
    float springBackTension = 0.5f;

    [SerializeField] AnimationCurve tensionOutputCurve, velocityDecayCurve;

    bool isReleasingTension;
    bool outputIsReceiving = true;

    [SerializeField] InputObject outputObject;
    [SerializeField] FloatSender objectChargedWith;
    IChargeInput chargeInput;

    protected override void OnEnable()
    {
        base.OnEnable();

        chargeInput = objectChargedWith.GetComponentInChildren<IChargeInput>();
        if (chargeInput != null)
        {
            chargeInput.OnStartChargeEvent += StartBuildingUpTension;
            chargeInput.OnEndChargeEvent += ReleaseTension;
        }
    }

    protected void OnDisable()
    {
        if (chargeInput != null)
        {
            chargeInput.OnStartChargeEvent -= StartBuildingUpTension;
            chargeInput.OnEndChargeEvent -= ReleaseTension;
        }
    }

    public override bool Try(float progress)
    {
        BuildUpTension(progress);
        return base.Try(progress);
    }

    private void StartBuildingUpTension()
    {
        //
    }

    private void BuildUpTension(float progress)
    {
        tension = progress;
    }

    [Button]
    private void ReleaseTension()
    {
        if (!isReleasingTension)
            StartCoroutine(TensionReleaseRoutine());
    }

    IEnumerator TensionReleaseRoutine()
    {
        isReleasingTension = true;
        float progression = 0;
        float startTension = tension;

        while (tension > 0f)
        {
            if (objectChargedWith != null)
                objectChargedWith.SendCallback(tension);

            progression += tensionOutputCurve.Evaluate(tension) * Time.deltaTime;
            tension -= Time.deltaTime;

            if (outputIsReceiving)
                outputObject.Try(progression);

            yield return null;
        }

        if (objectChargedWith != null)
        {
            if (startTension < springBackTension)
            {
                while (progression > 0)
                {
                    progression -= 1 * Time.deltaTime;

                    if (outputIsReceiving)
                        outputObject.Try(progression);

                    yield return null;
                }
            }
            else
            {
                outputIsReceiving = false;
            }
        }

        isReleasingTension = false;
    }
}
