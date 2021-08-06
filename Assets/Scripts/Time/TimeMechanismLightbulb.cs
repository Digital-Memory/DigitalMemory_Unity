using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMechanismLightbulb : SimpleAttachable
{
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Material lightbulbEmissive, metal_04;
    [SerializeField] GameObject shine;
    public void SetLightActive(bool active)
    {
        Material[] mats = meshRenderer.materials;
        mats[2] = active ? lightbulbEmissive : metal_04;
        meshRenderer.materials = mats;
        shine.SetActive(active);
    } 
}
