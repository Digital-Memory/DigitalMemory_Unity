using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeHandler : Singleton<TimeHandler>
{
    [SerializeField] private float time, timeMin, timeMax, timeStart, timeEnd;
    public float Time => time;

    [SerializeField] AudioClip rearrangeClip;
    [Expandable]
    [SerializeField] Effect resetEffect;

    public event System.Action<float> OnTimeUpdate;
    public event System.Action<float> OnTimeChange;
    public event System.Action<float> OnForceTimeReset;

    public void StartNewSequence(Sequence newSequence, bool startFromBeginning = true)
    {
        timeMin = newSequence.minTime;
        timeMax = newSequence.maxTime;
        timeStart = newSequence.startTime;
        timeEnd = newSequence.endTime;

        time = startFromBeginning ? newSequence.startTime : newSequence.startReversedTime;
        OnTimeUpdate?.Invoke(time);
        OnTimeChange?.Invoke(time);
    }

    public void IncreaseTime(int amount)
    {
        if (time + amount <= timeMax)
        {
            time += amount;
            Game.SoundPlayer.Play(rearrangeClip, null, volume: 0.25f, randomPitchRange: 0.5f);
            OnTimeUpdate?.Invoke(time);
            OnTimeChange?.Invoke(time);
        } else if (time + amount >= timeMax)
        {
            Debug.Log("time: " + time + " +" + amount + " = " + (time + amount) + " /  " + timeMax);
            Game.SequenceHandler.TryPlayAfter();
        }
    }

    public void ForceTimeSet(float newTime)
    {
        Debug.Log("Set Time to: " + newTime);

        Game.EffectHandler.Play(resetEffect, gameObject);
        time = newTime;
        OnTimeUpdate?.Invoke(newTime);
        OnForceTimeReset?.Invoke(newTime);
    }
    internal void MoveHand(float newTime)
    {
        time = newTime;
        OnTimeUpdate(newTime);
    }

    public void DecreaseTime(int amount)
    {
        if (time - amount >= timeMin)
        {
            time -= amount;
            Game.SoundPlayer.Play(rearrangeClip, null, volume: 0.25f, randomPitchRange: 0.5f);
            OnTimeUpdate(time);
            OnTimeChange(time);
        }
        else
        {
            if (time <= timeMin)
                Game.SequenceHandler.TryPlayBefore();
        }
    }
}
