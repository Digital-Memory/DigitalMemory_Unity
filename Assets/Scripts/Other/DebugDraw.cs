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

    public static void Cross(Vector3 position, Color color, float size)
    {
        Debug.DrawLine(position + new Vector3(size / 2, 0, size / 2), position + new Vector3(-size / 2, 0, -size / 2),color);
        Debug.DrawLine(position + new Vector3(-size / 2, 0, size / 2), position + new Vector3(size / 2, 0, -size / 2), color);
    }
}
