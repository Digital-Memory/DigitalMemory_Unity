using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ColliderControlingObject : ConditionedObject
{
    
    private enum ActionOnImpulseInput
    {
        Enable,
        Disable,
        Toggle,
    }

    [SerializeField] ActionOnImpulseInput actionOnImpulseInput;
    private new Collider collider;


    private void Awake()
    {
        collider = GetComponent<Collider>();
    }
    public override bool Try()
    {
        if (base.Try())
        {
            switch (actionOnImpulseInput)
            {
                case ActionOnImpulseInput.Toggle:
                    collider.enabled = !collider.enabled;
                    break;

                default:
                    collider.enabled = actionOnImpulseInput == ActionOnImpulseInput.Enable ? true : false;
                    break;
            }
            return true;
        }

        return false;
    }
}
