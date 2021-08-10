using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClickable
{
    bool IsClickable();
    void Click();

    public event System.Action OnClickEvent;
}

public class CubeRotatationOnClick : MonoBehaviour, IClickable
{
    [SerializeField] Cube cube;
    [SerializeField] Vector2 turnDirection;

    public event Action OnClickEvent;

    public void Click()
    {
        cube.TryTurn(turnDirection);
        OnClickEvent?.Invoke();
    }

    public bool IsClickable()
    {
        return !cube.IsTurning;
    }
}
