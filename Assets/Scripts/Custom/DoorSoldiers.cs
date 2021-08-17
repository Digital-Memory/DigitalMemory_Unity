using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSoldiers : BlendShapingObject
{
    AnimationCurve curve;
    [SerializeField] AudioClip teardownSound;
    [SerializeField] FloatSenderMoveBack moveBackCrank;
    bool isReceivingFloatInput = true;

    public override bool Try()
    {
        if (CheckAllConditionsForTrue())
        {
            isReceivingFloatInput = false;
            Time = 1f;
            Game.SoundPlayer.Play(teardownSound, volume: 0.66f, randomPitchRange: 0.1f);
            base.Try(false);
            return true;
        }
        return false;
    }

    public override bool Try(float progress)
    {
        if (!isReceivingFloatInput)
            return true;

        if (!CheckAllConditionsForTrue())
        {
            Time = 0;
            UpdateChange(progress);
            return true;
        }

        return true;
    }

    protected override void OnFinishedAnimating()
    {
        isReceivingFloatInput = true;
        base.OnFinishedAnimating();
    }

    protected override void UpdateChange(float progress)
    {
        base.UpdateChange(progress);
    }
}
