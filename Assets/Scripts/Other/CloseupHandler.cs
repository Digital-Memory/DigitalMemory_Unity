using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CloseupHandler : Singleton<CloseupHandler>
{
    Vector3 originalPosition;
    Quaternion originalRotation;

    Vector3 targetPosition;
    Quaternion targetRotation;

    Vector2 mousePositionBefore;
    bool offsetToTheLeftToMakeSpaceForInspectionText;

    [SerializeField] float closeupSpeed;
    [SerializeField] Transform closeupTransform;
    [SerializeField] AudioClip startCloseupSound, endCloseupSound;
    public void StartCloseup(ICloseupable currentCloseupable)
    {
        Debug.Log("start closeup");
        offsetToTheLeftToMakeSpaceForInspectionText = currentCloseupable.ShouldOffset();
        Game.SoundPlayer.Play(startCloseupSound, randomPitchRange: 0.15f);
        Game.MouseInteractor.ForceEndHover();
        currentCloseupable.OnStartCloseup();
        originalPosition = currentCloseupable.GetPosition();
        originalRotation = currentCloseupable.GetRotation();
    }

    public void EndCloseup(ICloseupable currentCloseupable)
    {
        Debug.Log("end closeup");
        Game.SoundPlayer.Play(endCloseupSound, randomPitchRange: 0.15f);
        StartCoroutine(PanBackRoutine(currentCloseupable));
    }
    public void UpdateCloseup(ICloseupable currentCloseupable)
    {
        targetPosition = closeupTransform.position - (offsetToTheLeftToMakeSpaceForInspectionText?closeupTransform.right:Vector3.zero);

        if (Input.GetMouseButton(0))
        {
            var x = mousePositionBefore.x - Input.mousePosition.x;
            var y = Input.mousePosition.y - mousePositionBefore.y;
            targetRotation = Quaternion.Euler(-x/2, x/2, y) * currentCloseupable.GetRotation();
        }

        mousePositionBefore = Input.mousePosition;
        UpdatePositionAndRotation(currentCloseupable, targetPosition, targetRotation, Vector3.Distance(targetPosition, currentCloseupable.GetPosition()) > 0.01f);
    }


    IEnumerator PanBackRoutine(ICloseupable closeupable)
    {
        Vector3 tPos = originalPosition;
        Quaternion tRot = originalRotation;

        while (Vector3.Distance(tPos, closeupable.GetPosition()) > 0.01f)
        {
            yield return null;
            UpdatePositionAndRotation(closeupable, tPos, tRot, lerp: true);
        }

        closeupable.UpdatePositionAndRotation(tPos, tRot);
        closeupable.OnEndCloseup();
    }

    private void UpdatePositionAndRotation(ICloseupable closeupable, Vector3 tPos, Quaternion tRot, bool lerp = false)
    {
        if (lerp)
        {
            Vector3 pos = Vector3.MoveTowards(closeupable.GetPosition(), tPos, Time.deltaTime * 100);
            Quaternion rot = Quaternion.Euler(Vector3.MoveTowards(closeupable.GetRotation().eulerAngles, tRot.eulerAngles, Time.deltaTime * 100));
            closeupable.UpdatePositionAndRotation(pos, rot);
        }
        else
        {
            closeupable.UpdatePositionAndRotation(tPos, tRot);
        }
    }
}
