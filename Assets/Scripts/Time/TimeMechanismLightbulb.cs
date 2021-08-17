using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMechanismLightbulb : SimpleAttachable
{
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Material lightbulbEmissive, metal_04;
    [SerializeField] GameObject shine;
    [SerializeField] [Foldout("Effects")] Effect onActiveEffect;

    public void SetLightActive(bool active)
    {
        if (shine.activeSelf == active)
            return;

        Material[] mats = meshRenderer.materials;
        mats[2] = active ? lightbulbEmissive : metal_04;
        meshRenderer.materials = mats;
        shine.SetActive(active);

        if (active)
            Game.EffectHandler.Play(onActiveEffect, gameObject);
    } 
}
