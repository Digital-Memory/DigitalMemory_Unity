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
        Debug.Log("start closeup");
        currentCloseupable = newCloseupable;
        offsetToTheLeftToMakeSpaceForInspectionText = newCloseupable.ShouldOffset();
        Game.SoundPlayer.Play(startCloseupSound, randomPitchRange: 0.15f);
        Game.HoverHandler.ForceEndHover();
        newCloseupable.OnStartCloseup();
        originalPosition = newCloseupable.GetPosition();
        originalRotation = newCloseupable.GetRotation();
    }

    public void EndCloseup(ICloseupable currentCloseupable)
    {
        Debug.Log("end closeup");
        Game.SoundPlayer.Play(endCloseupSound, randomPitchRange: 0.15f);
        StartCoroutine(PanBackRoutine(currentCloseupable));
    }
    public void UpdateCloseupMode(ICloseupable currentCloseupable)
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

    internal void UpdateCloseup(RaycastHit hit, bool v1, bool v2)
    {
        if (v2)
        {
            EndCloseup(currentCloseupable);
            currentCloseupable = null;
        }
        else
        {
            UpdateCloseupMode(currentCloseupable);

            if (v1)
            {
                HiddenAttachable hiddenAttachable = hit.collider.GetComponent<HiddenAttachable>();
                if (hiddenAttachable != null)
                {
                    EndCloseup(currentCloseupable);
                    currentCloseupable = hiddenAttachable;
                    StartCloseup(hiddenAttachable);
                }
            }
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
