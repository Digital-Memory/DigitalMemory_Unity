using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtentionHolder
{
    public static Vector2 To2D(this Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.z);
    }

    public static Vector3 To3D(this Vector2 vector2)
    {
        return new Vector3(vector2.x, 0, vector2.y);
    }

    public static Vector3 FilterByAxisVector(this Vector3 vector3, Vector3 axisVector)
    {
        return new Vector3(vector3.x * axisVector.x, vector3.y * axisVector.y, vector3.z * axisVector.z);
    }

    public static float FilterByAxis(this Vector3 vector3, Vector3 axisVector)
    {
        if (axisVector.x != 0f)
            return vector3.x;
        else if (axisVector.y != 0f)
            return vector3.y;
        else
            return vector3.z;
    }

    public static Vector3 InvertAxis (this Vector3 vector3, Vector3 axisVector)
    {
        return Vector3.Scale(vector3, Vector3.one - axisVector) + Vector3.Scale(-vector3, axisVector);
    }

    public static float GetLongestAxis(this Vector3 vector3)
    {
        float absX = Mathf.Abs(vector3.x);
        float absY = Mathf.Abs(vector3.y);
        float absZ = Mathf.Abs(vector3.z);

        if (absX > absY && absX > absZ)
        {
            return vector3.x;
        }
        else if (absY > absX && absY > absZ)
        {
            return vector3.y;
        }
        else
        {
            return vector3.z;
        }
    }

    public static float Min(this Vector3 vector3)
    {
        return Mathf.Min(vector3.x, vector3.y, vector3.z);
    }

    public static float Max(this Vector3 vector3)
    {
        return Mathf.Max(vector3.x, vector3.y, vector3.z);
    }

    public static Vector3 Clamp (this Vector3 vector3, Vector3 min, Vector3 max)
    {
        return new Vector3(Mathf.Clamp(vector3.x, min.x, max.x), Mathf.Clamp(vector3.y, min.y, max.y), Mathf.Clamp(vector3.z, min.z, max.z));
    }

    public static float[] NotZero(this Vector3 vector3)
    {
        List<float> floats = new List<float>();
        if (vector3.x != 0f) floats.Add(vector3.x);
        if (vector3.y != 0f) floats.Add(vector3.y);
        if (vector3.z != 0f) floats.Add(vector3.z);

        return floats.ToArray();
    }

    public static float MinWithoutZero(this Vector3 vector3)
    {
        float[] cleared = vector3.NotZero();

        switch (cleared.Length)
        {
            case 1: return cleared[0];
            case 2: return Mathf.Min(cleared[0], cleared[1]);
            case 3: return Mathf.Min(cleared[0], cleared[1], cleared[2]);
        }

        return float.MaxValue;
    }

    public static float MaxWithoutZero(this Vector3 vector3)
    {
        float[] cleared = vector3.NotZero();
        switch (cleared.Length)
        {
            case 1: return cleared[0];
            case 2: return Mathf.Max(cleared[0], cleared[1]);
            case 3: return Mathf.Max(cleared[0], cleared[1], cleared[2]);
        }

        return float.MinValue;
    }
}
