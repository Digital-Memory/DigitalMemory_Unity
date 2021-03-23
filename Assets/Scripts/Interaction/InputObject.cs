using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputObject : MonoBehaviour
{
    public virtual bool Try()
    {
        return false;
    }

    public virtual bool Try(bool on)
    {
        return false;
    }

    public virtual bool Try(float progress)
    {
        return false;
    }

}