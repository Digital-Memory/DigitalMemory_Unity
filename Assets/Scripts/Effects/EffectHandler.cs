using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectHandler : Singleton<EffectHandler>
{
    [Expandable]
    [SerializeField] Effect clearVisualEffects;

    Attacher[] attachers;
    Attacher[] Attachers
    {
        get
        {
            if (attachers == null)
                attachers = FindObjectsOfType<Attacher>();

            return attachers;
        }
    }
    internal void Play(Effect attachEffect, GameObject gameObject)
    {

        if (attachEffect != null && gameObject != null)
        {
            attachEffect.Play(gameObject);
        }
    }

    internal void PlayOnAllPotentialAttachables(Effect potentialSlotEffect, string attachment)
    {
        if (potentialSlotEffect != null)
            Array.ForEach(Attachers.Where(a => a.CanAttach(attachment)).ToArray(), x => Play(potentialSlotEffect, x.gameObject));
    }

    internal void StopOnAllPotentialAttachables(string attachment)
    {
        Array.ForEach(Attachers.Where(a => a.CanAttach(attachment)).ToArray(), x => Play(clearVisualEffects, x.gameObject));
    }
}
