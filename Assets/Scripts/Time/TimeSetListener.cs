using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSetListener : TimeListener
{
    int lastSet = 0;
    AnimatorStateInfo last;

    private void Update()
    {
        AnimatorStateInfo current = animator.GetCurrentAnimatorStateInfo(0);
        if (last.shortNameHash != current.shortNameHash)
        {
            Debug.Log("New State: " + current.shortNameHash);
            int time = GetTimeFromTagHash(current.tagHash);
            if (time != int.MinValue)
            {
                if ((int)Game.TimeHandler.Time != time)
                    Game.TimeHandler.ForceTimeSet(time);
            }

            last = current;
        }

    }
    private int GetTimeFromTagHash(int tagHash)
    {
        for (int i = 0; i <= 24; i++)
        {
            if (tagHash == Animator.StringToHash(i.ToString()))
                return i;
        }

        return int.MinValue;
    }
}
