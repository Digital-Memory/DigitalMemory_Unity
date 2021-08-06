using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

[CreateAssetMenu]
public class Effect : ScriptableObject
{
    [EnumFlags]
    [SerializeField]
    private EffectType effectType;

    private bool SoundEffect => effectType.HasFlag(EffectType.SoundEffect);
    private bool VisualEffect => effectType.HasFlag(EffectType.VisualEffect);
    private bool ChangeShaderEffect => effectType.HasFlag(EffectType.ChangeShaderEffect);
    private bool PulsingEffect => effectType.HasFlag(EffectType.PulsingEffect);
    private bool ChangeMaterialEffect => effectType.HasFlag(EffectType.ChangeMaterialEffect);
    private bool ChangeMaterialPropertyEffect => effectType.HasFlag(EffectType.ChangeMaterialPropertyEffect);


    [ShowIf("SoundEffect")] [Space] [Header("Sound")] [SerializeField] SoundEffectData soundEffect;

    [ShowIf("VisualEffect")] [Space] [Header("VisualEffect")] [SerializeField] private bool ClearVisualEffect;
    [ShowIf(EConditionOperator.And, "VisualEffect", "NotClearVisualEffect")] [SerializeField] VisualEffectData visualEffect;

    [ShowIf("ChangeShaderEffect")] [Space] [Header("ChangeShader")] [SerializeField] ChangeShaderEffectData changeShaderEffect;

    [ShowIf("ChangeMaterialEffect")] [Space] [Header("ChangeMaterial")] [SerializeField] private bool ClearChangeMaterialEffect;
    [ShowIf(EConditionOperator.And, "ChangeMaterialEffect", "NotClearMaterialEffect")] [Header("ChangeMaterial")] [SerializeField] ChangeMaterialEffectData changeMaterialEffect;

    [ShowIf("ChangeMaterialPropertyEffect")] [Space] [Header("ChangeMaterialProperty")] [SerializeField] ChangeMaterialPropertyEffectData changeMaterialPropertyEffect;

    [ShowIf("PulsingEffect")] [Space] [Header("PulsingEffect")] [SerializeField] private bool ClearPulsingEffect;
    [ShowIf(EConditionOperator.And, "PulsingEffect", "NotClearPulsingEffect")] [Header("PulsingEffect")] [SerializeField] PulsingEffectData pulsingEffect;

    public bool NotPlayPulsingEffect() { return !PulsingEffect; }
    public bool NotClearPulsingEffect() { return !ClearPulsingEffect; }
    public bool NotPlayVisualEffect() { return !VisualEffect; }
    public bool NotClearVisualEffect() { return !ClearVisualEffect; }
    public bool NotChangeMaterialEffect() { return !ChangeMaterialEffect; }
    public bool NotClearMaterialEffect() { return !ClearChangeMaterialEffect; }


    public void Play(GameObject origin)
    {
        //Debug.Log($"Play effect {name} on {((origin != null ) ? origin.name : "null")}");

        if (SoundEffect)
            soundEffect.PlayEffect(origin);

        if (VisualEffect)
        {
            if (ClearVisualEffect)
                ClearAllVisualEffectsFrom(origin);
            else
                visualEffect.PlayEffect(origin);
        }

        if (ChangeShaderEffect)
            changeShaderEffect.PlayEffect(origin);


        if (PulsingEffect)
        {
            if (ClearPulsingEffect)
                ClearAllPulsingEffectsFrom(origin);
            else
                pulsingEffect.PlayEffect(origin);
        }

        if (ChangeMaterialEffect)
        {
            if (ClearChangeMaterialEffect)
                ClearAllChangeMaterialEffectsFrom(origin);
            else
                changeMaterialEffect.PlayEffect(origin);
        }

        if (ChangeMaterialPropertyEffect)
            changeMaterialPropertyEffect.PlayEffect(origin);

    }

    private void ClearAllChangeMaterialEffectsFrom(GameObject origin)
    {
        ChangeMaterialEffector effector = origin.GetComponent<ChangeMaterialEffector>();

        if (effector != null)
            effector.Clear();
    }

    private void ClearAllPulsingEffectsFrom(GameObject origin)
    {
        foreach (PulsingEffector pulsing in origin.GetComponents<PulsingEffector>())
        {
            pulsing.Stop();
        }
    }

    private void ClearAllVisualEffectsFrom(GameObject origin)
    {
        foreach (VisualEffectInstance effect in origin.GetComponentsInChildren<VisualEffectInstance>())
        {
            effect.Destroy(0);
        }
    }

    private enum EffectType
    {
        None = 0,
        SoundEffect = 1 << 0,
        VisualEffect = 1 << 2,
        ChangeShaderEffect = 1 << 4,
        PulsingEffect = 1 << 6,
        ChangeMaterialEffect = 1 << 8,
        ChangeMaterialPropertyEffect = 1 << 10
    }
}

[System.Serializable]
public class SoundEffectData : EffectData
{
    public AudioClip clip;
    public bool playFromOrigin;
    public bool playOnlyIfFinished = false;
    public float volume = 1f;
    public float randomPitchRange = 0f;

    public override void PlayEffect(GameObject origin)
    {
        Game.SoundPlayer.Play(clip, playFromOrigin ? origin : null, volume, randomPitchRange, playOnlyIfFinished);
    }
}


[System.Serializable]
public class VisualEffectData : EffectData
{
    public Transform prefab;
    public bool spawnRelativeToOrigin;
    public bool spawnParented;
    public Vector3 spawnOffset;
    public float destroyDelay = -1f;

    public override void PlayEffect(GameObject origin)
    {
        Vector3 spawnPosition = (spawnRelativeToOrigin ? origin.transform.position : Vector3.zero) + spawnOffset;
        Transform effectInstance = GameObject.Instantiate(prefab, spawnPosition, Quaternion.identity, spawnParented ? origin.transform : null);
        effectInstance.gameObject.AddComponent<VisualEffectInstance>().Destroy(destroyDelay);
    }
}

[System.Serializable]
public class ChangeShaderEffectData : EffectData
{
    public Shader shader;
    public string boolName;
    public bool boolValue;

    public override void PlayEffect(GameObject origin)
    {
        foreach (var renderer in origin.GetComponentsInChildren<MeshRenderer>())
        {
            if (renderer.material.shader == shader)
            {
                renderer.material = new Material(renderer.material);
                renderer.material.SetInt(boolName, boolValue ? 1 : 0);
            }
        }
    }
}


[System.Serializable]
public class PulsingEffectData : EffectData
{
    public bool loopPulsing;
    public AnimationCurve pulseCurve;
    [EnumFlags]
    public PulsingEffectType type;

    public override void PlayEffect(GameObject origin)
    {
        PulsingEffector effector = origin.GetComponent<PulsingEffector>();
        if (effector == null)
        {
            effector = origin.AddComponent<PulsingEffector>();
        }

        if (loopPulsing)
            effector.StartPulsing(pulseCurve, type);
        else
        {
            effector.StartPulsing(pulseCurve[pulseCurve.length - 1].time, pulseCurve, type);
        }
    }
}

[System.Serializable]
public class ChangeMaterialEffectData : EffectData
{
    public Material ToChangeTo;

    public override void PlayEffect(GameObject origin)
    {
        ChangeMaterialEffector effector = origin.GetComponent<ChangeMaterialEffector>();
        if (effector != null)
            effector.UpdateChangedMaterialInChilren(ToChangeTo);
        else
            origin.AddComponent<ChangeMaterialEffector>().ChangeMaterialInChildren(ToChangeTo);
    }
}

[System.Serializable]
public class ChangeMaterialPropertyEffectData : EffectData
{
    public string BoolName;
    public bool BoolValue;

    public override void PlayEffect(GameObject origin)
    {
        //Need to improve this at some point
        MeshRenderer[] meshRenderers = origin.GetComponentsInChildren<MeshRenderer>();

        if (meshRenderers.Length == 0)
        {
            MeshRenderer inParent = origin.GetComponentInParent<MeshRenderer>();
            if (inParent != null)
                meshRenderers = new MeshRenderer[] { inParent };
        }

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.material.SetFloat(BoolName, BoolValue ? 1 : 0);
        }
    }
}

public class EffectData
{
    public virtual void PlayEffect(GameObject origin) { }
}
