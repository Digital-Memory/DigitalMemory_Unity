using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class TutorialFrame : MonoBehaviour
{
    CanvasGroup canvasGroup;

    private void OnEnable()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetOpacity(float opacity)
    {
        canvasGroup.alpha = opacity;
        
        if (opacity == 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(canvasGroup.alpha,1));
    }

    public void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(canvasGroup.alpha, 0));
    }

    private IEnumerator FadeRoutine(float current, float target)
    {
        bool postive = current < target;
        while (postive ? current < target : current > target)
        {
            current += (postive ? 1f : -1f) * Time.deltaTime * 2;
            SetOpacity(current);
            yield return null;
        }
    }
}
