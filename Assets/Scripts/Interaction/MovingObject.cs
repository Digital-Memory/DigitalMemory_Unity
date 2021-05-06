using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : ConditionedObject
{
    public Vector3 point1, point2;

    public override bool Try(float progress)
    {
        if (base.Try(progress))
        {
            UpdateMovement(progress);
            return true;
        }

        return false;
    }

    private void UpdateMovement(float progress)
    {
        transform.position = Vector3.Lerp(point1, point2, progress);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(point1, point2);
    }
}
