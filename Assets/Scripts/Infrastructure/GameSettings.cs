using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    public float DragDistance;
    public Vector3 AttachPreviewOffset;

    [Scene]
    public List<int> playableLevels = new List<int>();
    public float CurrentZoomLevel = 1;
    public Material DesaturationMaterial;
}
