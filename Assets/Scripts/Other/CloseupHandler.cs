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

    ICloseupable currentCloseupable;

    [SerializeField] float closeupSpeed;
    [SerializeField] Transform closeupTransform;
    [SerializeField] AudioClip startCloseupSound, endCloseupSound;

    public bool IsInCloseup { get => (currentCloseupable != null); }
    public void StartCloseup(ICloseupable newCloseupable)
    {
        currentCloseupable = newCloseupable;
        offsetToTheLeftToMakeSpaceForInspectionText = newCloseupable.ShouldOffset();
        Game.SoundPlayer.Play(startCloseupSound, randomPitchRange: 0.15f);
        Game.HoverHandler.ForceEndHoverCurrent();
        newCloseupable.OnStartCloseup();
        originalPosition = newCloseupable.GetPosition();
        originalRotation = newCloseupable.GetRotation();
    }

    public void EndCloseup(ICloseupable currentCloseupable)
    {
        Game.SoundPlayer.Play(endCloseupSound, randomPitchRange: 0.15f);
        StartCoroutine(PanBackRoutine(currentCloseupable));
    }
    public void UpdateCloseupMode(ICloseupable currentCloseupable)
    {
        targetPosition = closeupTransform.position - (offsetToTheLeftToMakeSpaceForInspectionText?closeupTransform.right:Vector3.zero) - currentCloseupable.GetCustomGlobalOffset();
        Debug.DrawLine(targetPosition, targetPosition + currentCloseupable.GetCustomGlobalOffset());

        if (Input.GetMouseButton(0))
        {
            var x = mousePositionBefore.x - Input.mousePosition.x;
            var y = Input.mousePosition.y - mousePositionBefore.y;
            targetRotation = Quaternion.Euler(-x, x, y*2) * currentCloseupable.GetRotation();
        }

        mousePositionBefore = Input.mousePosition;
        UpdatePositionAndRotation(currentCloseupable, targetPosition, targetRotation, Vector3.Distance(targetPosition, (currentCloseupable.GetPosition() - currentCloseupable.GetCustomGlobalOffset())) > 0.01f);
    }

    //Need to Improve this at some point
    internal void UpdateCloseup(RaycastHit hit, bool MouseButtonPressed)
    {
        if (MouseButtonPressed)
        {
            EndCloseup(currentCloseupable);
            currentCloseupable = null;
        }
        else
        {
            UpdateCloseupMode(currentCloseupable);
        }
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
