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

    public virtual void Try(InputType type, bool boolValue = true, float floatValue = 1f)
    {
        switch (type)
        {
            case InputType.Impulse:
                Try();
                break;

            case InputType.Bool:
                Try(boolValue);
                break;

            case InputType.Float:
                Try(floatValue);
                break;
        }
    }

    public static void Try(InputObject[] objects, InputType type, bool boolValue = true, float floatValue = 1f)
    {
        foreach (InputObject input in objects)
        {
            switch (type)
            {
                case InputType.Impulse:
                    input.Try();
                    break;

                case InputType.Bool:
                    input.Try(boolValue);
                    break;

                case InputType.Float:
                    input.Try(floatValue);
                    break;
            }

        }

    }
}