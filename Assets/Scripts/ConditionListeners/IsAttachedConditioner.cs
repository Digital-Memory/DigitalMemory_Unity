using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Attacher))]
public class IsAttachedConditioner : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] string variableName;

    [SerializeField] bool checkForSpecificAttachment;
    [SerializeField] [ShowIf("checkForSpecificAttachment")] string specificAttachment;


    [SerializeField] Effect onAttachEffect, onDetachEffect;
    [SerializeField] bool playEffectsOnlyAtTime;
    [SerializeField] [ShowIf("playEffectsOnlyAtTime")] float timeMinToPlayEffects;
    [SerializeField] [ShowIf("playEffectsOnlyAtTime")] float timeMaxToPlayEffects;

    Attacher attacher;
    float timeBefore = float.MinValue;
    private void OnEnable()
    {
        attacher = GetComponent<Attacher>();

        if (attacher != null)
        {
            attacher.OnChangeAttached += ChangedAttached;
        }
        else
        {
            Debug.LogWarning("No Attacher found.");
            Destroy(this);
        }
    }

    private void OnDisable()
    {
        if (attacher != null)
        {
            attacher.OnChangeAttached -= ChangedAttached;
        }
        else
        {
            Debug.LogWarning("No Attacher found.");
            Destroy(this);
        }
    }

    private void OnTimeUpdate(float newTime)
    {
        if ((timeBefore < timeMinToPlayEffects || timeBefore > timeMaxToPlayEffects) && newTime > timeMinToPlayEffects && newTime < timeMaxToPlayEffects)
            ChangedAttached(attacher.IsAttached, attacher.attachmentName);

        timeBefore = newTime;
    }


    void ChangedAttached(bool isAttached, string attachment)
    {
        if (checkForSpecificAttachment)
            isAttached = isAttached && attachment == specificAttachment;

        if (animator != null)
            animator.SetBool(variableName, isAttached);

        Game.EffectHandler.Play(isAttached ? onAttachEffect : onDetachEffect, gameObject);
    }
}
