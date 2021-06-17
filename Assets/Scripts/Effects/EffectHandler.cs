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
            //Need to Improve this at some point
            if (attachers == null)
                attachers = FindObjectsOfType<Attacher>();

            return attachers;
        }
    }
    internal void Play(Effect effect, GameObject gameObject)
    {

        if (effect != null && gameObject != null)
        {
            effect.Play(gameObject);
        }
    }

    internal void PlayOnAllPotentialAttachables(Effect potentialSlotEffect, string attachment)
    {
        if (potentialSlotEffect != null)
            Array.ForEach(Attachers.Where(a => a.CanAttach(attachment)).ToArray(), x => Play(potentialSlotEffect, x.gameObject));
    }

    internal void StopOnAllPotentialAttachables(string attachment)
    {
        Array.ForEach(Attachers.Where(a => a.attachmentName == attachment).ToArray(), x => Play(clearVisualEffects, x.gameObject));
    }
}
