using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionCube : MonoBehaviour, IClickable
{
    LevelSelection levelSelection;

    Vector3 startPosition;
    Quaternion startRotation;
    Vector3 targetPosition, targetRotation;

    Coroutine current;

    [SerializeField] AnimationCurve lerpCurve;
    [SerializeField] TextMesh textMesh;

    public event Action OnClickEvent;

    public void Init(LevelSelection levelSelection,int startIndex, string name)
    {
        this.levelSelection = levelSelection;
        textMesh.text = name;
        CalculatePositionAndRotationFromIndex(startIndex);
        PortToTarget();
    }

    public void CalculatePositionAndRotationFromIndex(int index)
    {
        targetPosition = new Vector3(0, Mathf.Pow(index, 2f) * 2, index * 14f);
        targetRotation = new Vector3(Mathf.Pow(index, 2f) * 10f, 0, 0);
    }

    public void Click()
    {
        levelSelection.ClickOn(this);
        OnClickEvent?.Invoke();
    }

    public bool IsClickable()
    {
        return true;
    }

    public void LerpToNewIndex(int index)
    {
        CalculatePositionAndRotationFromIndex(index);
        StopAllCoroutines();
        current = StartCoroutine(LerpToTarget());
    }

    public void PortToTarget()
    {
        transform.position = targetPosition;
        transform.rotation = Quaternion.Euler(targetRotation);
    }

    IEnumerator LerpToTarget()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        float t = lerpCurve[lerpCurve.length - 1].time;
        while ((t-=Time.deltaTime) >= 0)
        {
            float lerpAmount = lerpCurve.Evaluate(t);
            transform.position = Vector3.Lerp(targetPosition, startPosition, lerpAmount);
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(targetRotation), startRotation, lerpAmount);
            yield return null;
        }

        current = null;

        PortToTarget();
    }
}
