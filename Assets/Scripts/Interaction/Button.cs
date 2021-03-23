using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, IClickable
{
    bool isClicked;
    float clickedTimestamp;
    float heightAnimationDuration;

    [SerializeField] AnimationCurve buttonHeightOnClick;
    [SerializeField] Transform button;
    [SerializeField] Effect onClickEffect;

    public void Click()
    {
        isClicked = true;
        clickedTimestamp = Time.time;
        heightAnimationDuration = buttonHeightOnClick[buttonHeightOnClick.length - 1].time;

        Game.EffectHandler.Play(onClickEffect, gameObject);
    }

    public bool IsClickable()
    {
        return !isClicked;
    }

    private void Update()
    {
        if (isClicked)
        {
            if (Time.time > clickedTimestamp + heightAnimationDuration)
            {
                isClicked = false;
            } else
            {
                button.transform.localPosition = new Vector3(0, buttonHeightOnClick.Evaluate(Time.time - clickedTimestamp), 0);
            }
        }
    }
}
