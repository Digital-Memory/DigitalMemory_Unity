using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectToInventoryOnClick : MonoBehaviour, IClickable, IHoverable
{
    [SerializeField] InventoryObjectData dataToMoveToInventory;
    public bool IsNull => this == null;

    public event Action OnStartHoverEvent;
    public event Action OnEndHoverEvent;

    public void Click()
    {
        Game.UIHandler.InventoryAdder.MoveToInventory(dataToMoveToInventory, Input.mousePosition);
        Destroy(gameObject);
    }

    public void EndHover()
    {
        OnEndHoverEvent?.Invoke();
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public bool IsClickable()
    {
        return true;
    }

    public void StartHover()
    {
        OnStartHoverEvent?.Invoke();
    }
}
