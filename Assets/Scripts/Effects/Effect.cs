using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu]
public class Effect : ScriptableObject
{
    public bool PlaySoundEffect;
    [ShowIf("PlaySoundEffect")] [Header("Sound")] [SerializeField] SoundEffectData soundEffect;
    [Space]


    [ShowIf("NotClearVisualEffect")]
    public bool PlayVisualEffect;
    [ShowIf("PlayVisualEffect")] [Header("VisualEffect")] [SerializeField] VisualEffectData visualEffect;
    [ShowIf("NotPlayVisualEffect")] [Space] public bool ClearVisualEffect;
    [Space]


    public bool PlayChangeShaderEffect;
    [ShowIf("PlayChangeShaderEffect")] [Header("ChangeShader")] [SerializeField] ChangeShaderEffectData changeShaderEffect;
    [Space]


    [ShowIf("NotClearPulsingEffect")] public bool PlayPulsingEffect;
    [ShowIf(EConditionOperator.And, "PlayPulsingEffect", "NotClearPulsingEffect")] [Header("PulsingEffect")] [SerializeField] PulsingEffectData pulsingEffect;
    [ShowIf("NotPlayPulsingEffect")] [Space] public bool ClearPulsingEffect;

    public bool NotPlayPulsingEffect() { return !PlayPulsingEffect; }
    public bool NotClearPulsingEffect() { return !ClearPulsingEffect; }

    public bool NotPlayVisualEffect() { return !PlayVisualEffect; }
    public bool NotClearVisualEffect() { return !ClearVisualEffect; }


    public void Play(GameObject origin)
    {
        if (PlaySoundEffect)
            soundEffect.PlayEffect(origin);

        if (ClearVisualEffect)
            ClearAllVisualEffectsFrom(origin);

        if (PlayVisualEffect)
            visualEffect.PlayEffect(origin);

        if (PlayChangeShaderEffect)
            changeShaderEffect.PlayEffect(origin);

        if (ClearPulsingEffect)
            ClearAllPulsingEffectsFrom(origin);

        if (PlayPulsingEffect)
            pulsingEffect.PlayEffect(origin);

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
            effect.Destroy();
        }
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
    public Vector3 spawnOffset;
    public float destroyDelay = -1f;

    public override void PlayEffect(GameObject origin)
    {
        Vector3 spawnPosition = (spawnRelativeToOrigin ? origin.transform.position : Vector3.zero) + spawnOffset;
        Transform effectInstance = GameObject.Instantiate(prefab, spawnPosition, Quaternion.identity, origin.transform);
        effectInstance.gameObject.AddComponent<VisualEffectInstance>().DestroyDelayed(destroyDelay);
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

public class EffectData
{
    public virtual void PlayEffect(GameObject origin)
    {
        //
    }
}
