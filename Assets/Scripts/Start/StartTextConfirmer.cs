using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartTextConfirmer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Graphic graphic;
    [SerializeField] bool blink;

    private void OnEnable()
    {
        StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        float alpha = blink ? graphic.color.a : 0f;
        while (blink ? true : (alpha < 1))
        {
            alpha = blink ? Mathf.MoveTowards(alpha, (Mathf.Sin(Time.time * 5) + 1) / 2, Time.deltaTime) : (alpha += Time.deltaTime);
            Color c = new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha);
            if (graphic) graphic.color = c;
            yield return null;
        }
    }

    private void OnDisable()
    {
        Color c = new Color(graphic.color.r, graphic.color.g, graphic.color.b, 0);
        if (graphic) graphic.color = c;
        StopAllCoroutines();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, 1);
        StopAllCoroutines();
        transform.localScale = new Vector2(1.1f, 1.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(BlinkRoutine());
        transform.localScale = Vector2.one;
    }
}
