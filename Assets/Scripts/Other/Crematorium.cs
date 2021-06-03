using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crematorium : InputObject
{
    [ShowNonSerializedField] bool isLeft = false;
    [SerializeField] InputObject leftObject, rightObject;
    [SerializeField] AnimatingObject worker;

    public override bool Try(float progress)
    {
        if (progress < 0.05 && isLeft == false)
        {
            FireInput(left: true);
            isLeft = true;
        }
        else if (progress > 0.95 && isLeft == true)
        {
            FireInput(left: false);
            isLeft = false;
        }

        worker?.Try(progress);

        return true;
    }

    private void FireInput(bool left)
    {
        (left ? leftObject : rightObject).Try(true);
    }
}
