using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatch : MonoBehaviour
{
    [SerializeField] Transform doorRight, doorLeft;
    [SerializeField] AnimationCurve doorZRotation;

    bool isOpen;
    bool isAnimating;
    float clickedTimestamp;
    float rotationAnimationDuration;


    [Button]
    public void Open()
    {
        isAnimating = true;
        clickedTimestamp = Time.time;
        rotationAnimationDuration = doorZRotation[doorZRotation.length - 1].time;
        isOpen = true;
    }

    [Button]
    public void Close()
    {
        isAnimating = true;
        clickedTimestamp = Time.time;
        rotationAnimationDuration = doorZRotation[doorZRotation.length - 1].time;
        isOpen = false;
    }

    public bool IsClickable()
    {
        return !isAnimating;
    }

    private void Update()
    {
        if (isAnimating)
        {
            if (Time.time > clickedTimestamp + rotationAnimationDuration)
            {
                isAnimating = false;
            }
            else
            {
                float rotation = doorZRotation.Evaluate(isOpen ? (Time.time - clickedTimestamp) : ((Time.time - clickedTimestamp) * -1));
                doorLeft.localRotation = Quaternion.Euler(0, 0, rotation);
                doorRight.localRotation = Quaternion.Euler(0, 0, -rotation);
            }
        }
    }
}
