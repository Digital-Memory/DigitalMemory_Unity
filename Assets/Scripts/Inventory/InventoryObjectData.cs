using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Linq;

[CreateAssetMenu]
public class InventoryObjectData : ScriptableObject
{

    [ShowAssetPreview(128, 128)]
    public GameObject prefab;

    [ShowAssetPreview(128, 128)]
    public Sprite icon;

    public float overviewSceneScaleMultiplier = 1f, zoomInSceneScaleMultiplier = 0.2f;
    public float overviewSceneDistance = 3, zoomInSceneDistance = 1f;

    [ResizableTextArea]
    public string hoverText;
}
