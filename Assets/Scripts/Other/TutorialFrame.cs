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
        canvasGroup.alpha = 0f;
    }

    public void SetOpacity(float opacity)
    {
        canvasGroup.alpha = opacity;
        
        if (opacity == 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void FadeIn(float duration = 1f, float delay = 0f)
    {
        if (!isActiveAndEnabled)
            gameObject.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(FadeRoutine(canvasGroup.alpha, 1, duration, delay));
    }

    public void FadeOut(float duration = 1f, float delay = 0f)
    {
        if (!isActiveAndEnabled)
            return;

        StopAllCoroutines();
        StartCoroutine(FadeRoutine(canvasGroup.alpha, 0, duration, delay));
    }

    private IEnumerator FadeRoutine(float current, float target, float duration, float delay)
    {
        if (delay > 0)
            yield return new WaitForSeconds(delay);

        bool postive = current < target;
        while (postive ? current < target : current > target)
        {
            current += (postive ? 1f : -1f) * (Time.deltaTime / duration) * 2;
            SetOpacity(current);
            yield return null;
        }
    }
}
