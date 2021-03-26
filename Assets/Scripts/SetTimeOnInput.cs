using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTimeOnInput : ConditionedObject
{
    [SerializeField] TimePoint time;

    public override bool Try()
    {
        if (base.Try()) {
            Game.TimeHandler.SetTime(time);
            return true;
        }

        return false;
    }
}
