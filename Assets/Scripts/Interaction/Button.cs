using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : InputSender, IClickable, IInputSender, IHoverable
{
    bool isClicked;
    float clickedTimestamp;
    float heightAnimationDuration;

    [SerializeField] AnimationCurve buttonHeightOnClick;
    [SerializeField] Transform button;
    [SerializeField] Effect onClickEffect;

    public bool IsNull => this == null;

    public event Action OnStartHoverEvent;
    public event Action OnEndHoverEvent;

    public void Click()
    {
        isClicked = true;
        clickedTimestamp = Time.time;
        heightAnimationDuration = buttonHeightOnClick[buttonHeightOnClick.length - 1].time;

        Game.EffectHandler.Play(onClickEffect, gameObject);
        if (input != null)
        {
            if (input.Try(true))
            {
                CallOnSendInputEvents(0f);
            }
        } else
        {
            Debug.LogWarning("No Input " + input);
        }

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
            }
            else
            {
                button.transform.localPosition = new Vector3(0, buttonHeightOnClick.Evaluate(Time.time - clickedTimestamp), 0);
            }

            DebugDraw.Circle(transform.position + Vector3.up, Color.yellow, 1);
            if (inputObject != null)
                Debug.DrawLine(transform.position + Vector3.up, inputObject.transform.position + Vector3.up, Color.yellow);
        }
        else
        {
            if (inputObject != null)
                Debug.DrawLine(transform.position + Vector3.up, inputObject.transform.position + Vector3.up, Color.grey);
        }
    }

    public void StartHover()
    {
        OnStartHoverEvent?.Invoke();
    }

    public void EndHover()
    {
        OnEndHoverEvent?.Invoke();
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
