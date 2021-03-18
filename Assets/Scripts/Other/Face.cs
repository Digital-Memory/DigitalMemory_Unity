using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Face : ScriptableObject
{
    public Face left, right, up, down;
    public Quaternion direction;
    public Vector3 position;
}
