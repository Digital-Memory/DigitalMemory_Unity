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

}
