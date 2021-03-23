using NaughtyAttributes;
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

    [OnValueChanged("OnChangeInputObject")]
    [SerializeField] InputObject inputObject;

    [ShowIf("inputObjectGameObjectIsCorrect")]
    [ShowAssetPreview(128, 128)]
    [SerializeField] GameObject inputObjectGameObject;
    [HideInInspector] public bool inputObjectGameObjectIsCorrect { get => (inputObjectGameObject != null && inputObjectGameObject == inputObject.gameObject); }

    private void OnChangeInputObject()
    {
        inputObjectGameObject = inputObject.gameObject;
    }

    public void Click()
    {
        isClicked = true;
        clickedTimestamp = Time.time;
        heightAnimationDuration = buttonHeightOnClick[buttonHeightOnClick.length - 1].time;

        Game.EffectHandler.Play(onClickEffect, gameObject);
        if (inputObject != null)
            inputObject.Try(true);
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
}
