using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialEffector : MonoBehaviour
{
    Dictionary<MeshRenderer, Material> before = new Dictionary<MeshRenderer, Material>();

    internal void Clear()
    {
        foreach (KeyValuePair<MeshRenderer, Material> materialBefore in before)
        {
            materialBefore.Key.material = materialBefore.Value;
        }

        Destroy(this);
    }

    internal void ChangeMaterialInChildren(Material toChangeTo)
    {
        foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>())
        {
            before.Add(meshRenderer,meshRenderer.material);
            meshRenderer.material = toChangeTo;
        }
    }

    internal void UpdateChangedMaterialInChilren(Material toChangeTo)
    {
        foreach (KeyValuePair<MeshRenderer, Material> materialBefore in before)
        {
            materialBefore.Key.material = toChangeTo;
        }
    }
}
