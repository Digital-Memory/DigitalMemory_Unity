using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClickable
{
    bool IsClickable();
    void Click();
}

public class CubeRotatationOnClick : MonoBehaviour, IClickable
{
    [SerializeField] Cube cube;
    [SerializeField] Vector2 turnDirection;
   
    public void Click()
    {
        cube.TryTurn(turnDirection);
    }

    public bool IsClickable()
    {
        return !cube.IsTurning;
    }
}
