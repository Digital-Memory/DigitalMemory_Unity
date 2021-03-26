using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] AnimationCurve xPosChangeByMouseX, zPosChangeByMouseY, xRotChangeByMouseX, zRotChangeByMouseY;

    [SerializeField] Transform lookAround;
    [SerializeField] AnimationCurve curveRotation;
    [SerializeField] Face current;

    bool isRotating = false;

    // Update is called once per frame
    void Update()
    {
        if (Game.CloseupHandler.IsInCloseup)
            return;

        var x = Input.mousePosition.x / Screen.width;
        var y = Input.mousePosition.y / Screen.height;

        lookAround.localPosition = xPosChangeByMouseX.Evaluate(x) * Vector3.forward + zPosChangeByMouseY.Evaluate(y) * Vector3.left;
        lookAround.localRotation = Quaternion.Euler(xRotChangeByMouseX.Evaluate(x), 0, zRotChangeByMouseY.Evaluate(y));

        if (Input.GetMouseButtonDown(0))
        {
            if (x < 0.05f)
                Rotate(current.left);
            else if (x > 0.95f)
                Rotate(current.right);
            else if (y < 0.05f)
                Rotate(current.down);
            else if (y > 0.95f)
                Rotate(current.up);
        }
    }

    public Vector3 GetUpVector()
    {
        return transform.up;
    }

    void Rotate(Face face)
    {
        if (!isRotating)
        {
            StartCoroutine(RotateRoutine(face));
            isRotating = true;
        }
    }

    IEnumerator RotateRoutine(Face face)
    {
        Quaternion rotationBefore = transform.rotation;
        Vector3 positionBefore = transform.position;

        float timeMax = curveRotation.keys[curveRotation.length - 1].time;
        float valueMax = curveRotation.keys[curveRotation.length - 1].value;
        float t = 0;

        while (t <= timeMax)
        {
            float lerp = curveRotation.Evaluate(t) / valueMax;
            transform.rotation = Quaternion.Lerp(rotationBefore, face.direction, lerp);
            transform.position = Vector3.Lerp(positionBefore, face.position, lerp);
            t += Time.deltaTime;
            yield return null;
        }
        //transform.rotation = Quaternion.Euler(RoundToFullRotation(transform.rotation.eulerAngles));
        current = face;
        isRotating = false;
    }

    private void OnGUI()
    {
        GUI.Button(new Rect(0, 0, Screen.width, Screen.height * 0.05f),"");
        GUI.Button(new Rect(0, Screen.height * 0.95f, Screen.width, Screen.height * 0.05f), "");
        GUI.Button(new Rect(0, 0, Screen.width * 0.05f, Screen.height), "");
        GUI.Button(new Rect(Screen.width * 0.95f, 0, Screen.width * 0.05f, Screen.height), "");
    }
}
