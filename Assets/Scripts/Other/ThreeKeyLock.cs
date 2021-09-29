using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeKeyLock : MonoBehaviour
{
    SkinnedMeshRenderer lockMeshRenderer;
    AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 100);
    Attacher[] attachers;
    int stagesSolved = 0;

    private void OnEnable()
    {
        lockMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        attachers = GetComponentsInChildren<Attacher>();

        foreach (Attacher attacher in attachers)
        {
            attacher.OnChangeAttached += OnChangeAttached;
        }
    }

    private void OnDisable()
    {
        foreach (Attacher attacher in attachers)
        {
            attacher.OnChangeAttached -= OnChangeAttached;
        }
    }

    private void OnChangeAttached(bool isAttached, string attachment)
    {
        if (isAttached)
        {
            stagesSolved++;
            StartCoroutine(AnimateStageRoutine(stagesSolved - 1));
        }
    }


    IEnumerator AnimateStageRoutine(int stage)
    {

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime;
            lockMeshRenderer.SetBlendShapeWeight(stage, curve.Evaluate(t));
            yield return null;
        }
    }
}
