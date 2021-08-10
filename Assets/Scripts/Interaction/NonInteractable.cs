using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NonInteractable : MonoBehaviour, IClickable
{
    [SerializeField] bool playEffect = true;
    [Expandable] [SerializeField] Effect nonInteractable;

    [SerializeField] bool playSound = false;
    [SerializeField] AudioClip sound;

    public event Action OnClickEvent;

    public void Click()
    {
        if (playEffect) Game.EffectHandler.Play(nonInteractable, gameObject);

        else if (playSound) Game.SoundPlayer.Play(sound);

        OnClickEvent?.Invoke();

    }

    public bool IsClickable()
    {
        return true;
    }

}
