using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : ChangingOverTimeObject
{
    [OnValueChanged("ChangeObjectToFollow")]
    [SerializeField] Transform objectToMove;
    [OnValueChanged("PreviewTruePosition")]
    [SerializeField] Vector3 localPositionTrue;
    [OnValueChanged("PreviewFalsePosition")]
    [SerializeField] Vector3 localPositionFalse;

#if UNITY_EDITOR

    protected override void Reset()
    {
        base.Reset();

        if (transform.childCount > 0)
            objectToMove = transform.GetChild(0);

        if (objectToMove != null)
        {
            localPositionTrue = objectToMove.localPosition;
            localPositionFalse = objectToMove.localPosition;
        }
    }

    private void ChangeObjectToFollow()
    {
        if (objectToMove != null)
        {
            localPositionTrue = objectToMove.localPosition;
            localPositionFalse = objectToMove.localPosition;
        }   
    }

    private void PreviewTruePosition()
    {
        objectToMove.localPosition = localPositionTrue;
    }

    private void PreviewFalsePosition()
    {
        objectToMove.localPosition = localPositionFalse;
    }

#endif

    protected override void UpdateChange(float progress)
    {
        objectToMove.localPosition = Vector3.Lerp(localPositionFalse, localPositionTrue, progress);
    }

    private void OnDrawGizmos()
    {
        if (objectToMove != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine((objectToMove.parent.position) + localPositionTrue, ((objectToMove.parent.position) + localPositionFalse));
        }
    }
}
