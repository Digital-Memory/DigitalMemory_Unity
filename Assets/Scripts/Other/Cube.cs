using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] Transform rotator;
    [SerializeField] AnimationCurve turningCurve;
    public bool IsTurning { get; internal set; }

    // Start is called before the first frame update
    void Start()
    {
        IsTurning = false;
    }

    internal void TryTurn(Vector2 turnDirection)
    {
        if (IsTurning)
            return;

        
        StartCoroutine(TurnRoutine(turnDirection));
    }

    private IEnumerator TurnRoutine(Vector2 turnDirection)
    {
        Debug.Log("start turning to " + turnDirection);

        IsTurning = true;
        Vector3 targetRotation = rotator.rotation.eulerAngles + turnDirection.x * Vector3.forward * 90f + turnDirection.y * Vector3.right * 90f;

        float t = 0;

        while ((t+=Time.deltaTime) < 1f)
        {
            rotator.rotation = Quaternion.Euler(Vector3.Lerp(rotator.rotation.eulerAngles, targetRotation, turningCurve.Evaluate(t)));
            yield return null;
        }

        Debug.Log("finished turning to " + turnDirection);

        IsTurning = false;
    }
}
