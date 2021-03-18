using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class TimeChangeButton : MonoBehaviour, IClickable
{
    [SerializeField] bool increase;
    [SerializeField] int timeChangeAmount;
    [SerializeField] Animator buttonAnimator;
    [SerializeField] AudioClip clickSound;

    public void Click()
    {
        Game.SoundPlayer.Play(clickSound);

        if (increase)
            Game.TimeHandler.IncreaseTime(timeChangeAmount);
        else
            Game.TimeHandler.DecreaseTime(timeChangeAmount);

        buttonAnimator.SetTrigger("OnClick");
    }

    public bool IsClickable()
    {
        return buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
    }
}
