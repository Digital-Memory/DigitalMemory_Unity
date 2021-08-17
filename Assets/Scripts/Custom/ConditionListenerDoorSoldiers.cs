using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DoorSoldiers))]
public class ConditionListenerDoorSoldiers : ConditionListenerBehaviour
{
    private enum DoorSoldierState
    {
        Uncharged,
        Charged,
        Crashed,
    }

    DoorSoldiers doorSoldiers;
    DoorSoldierState state;

    private void OnEnable()
    {
        doorSoldiers = GetComponent<DoorSoldiers>();

        if (doorSoldiers != null)
        {
            doorSoldiers.OnTimeChangeEvent += OnSoldiersMove;
            doorSoldiers.OnFinishedAnimatingEvent += OnSoldiersHit;

        }
    }
    private void OnSoldiersMove(float time)
    {
        if (state == DoorSoldierState.Uncharged && time > 0.5f)
            state = DoorSoldierState.Charged;
    }

    private void OnSoldiersHit()
    {
        if (state == DoorSoldierState.Charged)
            state = DoorSoldierState.Crashed;
    }

    private void OnDisable()
    {
        if (doorSoldiers != null)
        {
            doorSoldiers.OnTimeChangeEvent -= OnSoldiersMove;
            doorSoldiers.OnFinishedAnimatingEvent -= OnSoldiersHit;
        }
    }

    public override bool SupportsBool()
    {
        return true;
    }

    public override bool GetBool()
    {
        return state == DoorSoldierState.Crashed;
    }
}
