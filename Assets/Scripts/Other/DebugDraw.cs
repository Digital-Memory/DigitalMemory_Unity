using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugDraw
{
    public static void Circle(Vector3 position, Color color, float size, int segmentLength = 15)
    {
        Vector3 lastPosition = Vector3.zero;

        for (int i = 0; i <= 360; i+= segmentLength)
        {
            Vector2 positionXY = new Vector2(Mathf.Sin(i * Mathf.Deg2Rad), Mathf.Cos(i * Mathf.Deg2Rad));
            Vector3 positionxyz = new Vector3(position.x + positionXY.x * size, position.y, position.z + positionXY.y * size);

            if (lastPosition != Vector3.zero)
            {
                Debug.DrawLine(lastPosition, positionxyz, color);
            }

            lastPosition = positionxyz;
        }
    }

    public static void Plane(Vector3 position, Vector3 normal)
    {
        Vector3 v3;

        if (normal.normalized != Vector3.forward)
            v3 = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
        else
            v3 = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude; ;

        var corner0 = position + v3;
        var corner2 = position - v3;
        var q = Quaternion.AngleAxis(90f, normal);
        v3 = q * v3;
        var corner1 = position + v3;
        var corner3 = position - v3;

        Debug.DrawLine(corner0, corner2, Color.green);
        Debug.DrawLine(corner1, corner3, Color.green);
        Debug.DrawLine(corner0, corner1, Color.green);
        Debug.DrawLine(corner1, corner2, Color.green);
        Debug.DrawLine(corner2, corner3, Color.green);
        Debug.DrawLine(corner3, corner0, Color.green);
        Debug.DrawRay(position, normal, Color.red);
    }

    public static void Cross(Vector3 position, Color color, float size)
    {
        Debug.DrawLine(position + new Vector3(size / 2, 0, size / 2), position + new Vector3(-size / 2, 0, -size / 2),color);
        Debug.DrawLine(position + new Vector3(-size / 2, 0, size / 2), position + new Vector3(size / 2, 0, -size / 2), color);
    }
}
